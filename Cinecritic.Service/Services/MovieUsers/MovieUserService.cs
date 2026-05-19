using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Domain.Models;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Cinecritic.Application.Services.MovieUsers
{
    public class MovieUserService : IMovieUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieUserRepository _movieUserRepository;
        private readonly IWatchListRepository _watchListRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;

        private static CancellationTokenSource _resetUserCacheToken = new CancellationTokenSource();
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public MovieUserService(
            IUnitOfWork unitOfWork, 
            IMovieUserRepository movieUserRepository,
            IWatchListRepository watchListRepository,
            IMapper mapper, 
            IFileService fileService, 
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _movieUserRepository = movieUserRepository;
            _watchListRepository = watchListRepository;
            _mapper = mapper;
            _fileService = fileService;
            _cache = cache;
        }

        public async Task<Result<MovieUserStatusDto>> RateMovieAsync(RateMovieDto dto)
        {
            var movieUser = await _movieUserRepository.GetMovieUserWithReviewAsync(dto.MovieId, dto.ApplicationUserId);

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(dto.MovieId, dto.ApplicationUserId);
                movieUser = _mapper.Map<MovieUser>(dto);
                _movieUserRepository.Add(movieUser);
            }
            else
            {
                movieUser.Rate = dto.Rate;
            }

            await _unitOfWork.CommitAsync();
            
            ClearCache();

            var resultDto = _mapper.Map<MovieUserStatusDto>(movieUser);
            resultDto.IsWatched = true;
            return Result.Ok(resultDto);
        }

        public async Task<Result<MovieUserStatusDto>> ToggleWatchMovieAsync(int movieId, int userId)
        {
            var movieUser = await _movieUserRepository.GetMovieUserWithReviewAsync(movieId, userId);
            bool isWatched = false;

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(movieId, userId);
                _movieUserRepository.Add(new MovieUser { MovieId = movieId, ApplicationUserId = userId });
                isWatched = true;
            }
            else
            {
                _movieUserRepository.Delete(movieUser);
            }

            await _unitOfWork.CommitAsync();

            ClearCache();

            return Result.Ok(new MovieUserStatusDto
            {
                ApplicationUserId = userId,
                IsWatched = isWatched
            });
        }

        public async Task<Result<MovieUserStatusDto>> ToggleLikeMovieAsync(int movieId, int userId)
        {
            var movieUser = await _movieUserRepository.GetMovieUserWithReviewAsync(movieId, userId);
    
            bool nextLikedState = movieUser == null || !movieUser.IsLiked;

            await _movieUserRepository.UpsertMovieUserLikeAndRatingAsync(movieId, userId.ToString(), nextLikedState, movieUser?.Rate);
            _unitOfWork.ClearTracker();

            ClearCache();

            return Result.Ok(new MovieUserStatusDto
            {
                ApplicationUserId = userId,
                IsWatched = true,
                IsLiked = nextLikedState,
                Rate = movieUser?.Rate,
                ReviewText = movieUser?.Review?.ReviewText,
                ReviewDate = movieUser?.Review?.ReviewDateTime.Date != null 
                    ? DateOnly.FromDateTime(movieUser.Review.ReviewDateTime.Date) 
                    : null
            });
        }

        public async Task DeleteFromWatchListAsync(int movieId, int userId)
        {
            var watchList = await _watchListRepository.GetAsync(movieId, userId);
            if (watchList != null)
            {
                _watchListRepository.Delete(watchList);
            }
        }

        public async Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(int userId, int pageSize, int pageCount)
        {
            var cacheKey = $"watched_movies_user_{userId}_page_{pageCount}_size_{pageSize}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetUserCacheToken.Token));

                var movies = await _movieUserRepository.GetWatchedMoviesAsync(userId, pageSize, pageCount);
                foreach (var movie in movies)
                {
                    movie.ImagePath = GetFilePath(movie.Id);
                }
                var dto = new GetMoviesResultDto
                {
                    Movies = movies,
                    TotalMovieNumber = await _movieUserRepository.CountWatched(userId)
                };
                return Result.Ok(dto);
            }) ?? Result.Fail<GetMoviesResultDto>("Error loading watched movies from cache");
        }

        public async Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(int userId, int pageSize, int pageCount)
        {
            var cacheKey = $"liked_movies_user_{userId}_page_{pageCount}_size_{pageSize}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetUserCacheToken.Token));

                var movies = await _movieUserRepository.GetLikedMoviesAsync(userId, pageSize, pageCount);
                foreach (var movie in movies)
                {
                    movie.ImagePath = GetFilePath(movie.Id);
                }
                var dto = new GetMoviesResultDto
                {
                    Movies = movies,
                    TotalMovieNumber = await _movieUserRepository.CountLiked(userId)
                };
                return Result.Ok(dto);
            }) ?? Result.Fail<GetMoviesResultDto>("Error loading liked movies from cache");
        }

        private string GetFilePath(int movieId)
        {
            var result = _fileService.GetFilePath(Path.Combine(MovieService.MoviePath, $"{movieId}.jpg"));
            return result.IsSuccess ? result.Value : "/images/no-image.webp";
        }

        private void ClearCache()
        {
            var currentToken = _resetUserCacheToken;
            _resetUserCacheToken = new CancellationTokenSource();
            currentToken.Cancel();
            currentToken.Dispose();
            
            MovieService.ClearMovieCache(); 
        }
    }
}