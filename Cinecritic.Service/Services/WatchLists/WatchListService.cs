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

namespace Cinecritic.Application.Services.WatchLists
{
    public class WatchListService : IWatchListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWatchListRepository _watchListRepository;
        private readonly IMovieUserRepository _movieUserRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IMemoryCache _cache;

        private static CancellationTokenSource _resetWatchListCacheToken = new CancellationTokenSource();
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public WatchListService(
            IUnitOfWork unitOfWork,
            IWatchListRepository watchListRepository,
            IMovieUserRepository movieUserRepository,
            IMapper mapper,
            IFileService fileService,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _watchListRepository = watchListRepository;
            _movieUserRepository = movieUserRepository;
            _mapper = mapper;
            _fileService = fileService;
            _cache = cache;
        }

        public async Task<Result<MovieUserStatusDto>> ToggleWatchListMovieAsync(int movieId, int userId)
        {
            bool isInWatchList = false;
            var watchList = await _watchListRepository.GetAsync(movieId, userId);

            if (watchList == null)
            {
                await DeleteFromMovieUserAsync(movieId, userId);
                _watchListRepository.Add(new WatchList { MovieId = movieId, ApplicationUserId = userId });
                isInWatchList = true;
            }
            else
            {
                _watchListRepository.Delete(watchList);
            }

            await _unitOfWork.CommitAsync();

            ClearCache();

            return Result.Ok(new MovieUserStatusDto
            {
                ApplicationUserId = userId,
                IsInWatchList = isInWatchList
            });
        }

        private async Task DeleteFromMovieUserAsync(int movieId, int userId)
        {
            var movieUser = await _movieUserRepository.GetMovieUserWithReviewAsync(movieId, userId);
            if (movieUser != null)
            {
                _movieUserRepository.Delete(movieUser);
            }
        }

        public async Task<Result<GetMoviesResultDto>> GetMoviesInWatchListAsync(int userId, int pageSize, int pageCount)
        {
            var cacheKey = $"watchlist_movies_user_{userId}_page_{pageCount}_size_{pageSize}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetWatchListCacheToken.Token));

                var movies = await _watchListRepository.GetMoviesInWatchListAsync(userId, pageSize, pageCount);
                foreach (var movie in movies)
                {
                    movie.ImagePath = GetFilePath(movie.Id);
                }
                var dto = new GetMoviesResultDto
                {
                    Movies = movies,
                    TotalMovieNumber = await _watchListRepository.Count(userId)
                };
                return Result.Ok(dto);
            }) ?? Result.Fail<GetMoviesResultDto>("Error loading watchlist from cache");
        }

        private string GetFilePath(int movieId)
        {
            var result = _fileService.GetFilePath(Path.Combine(MovieService.MoviePath, $"{movieId}.jpg"));
            return result.IsSuccess ? result.Value : "/images/no-image.webp";
        }

        private void ClearCache()
        {
            var currentToken = _resetWatchListCacheToken;
            _resetWatchListCacheToken = new CancellationTokenSource();
            currentToken.Cancel();
            currentToken.Dispose();

            MovieService.ClearMovieCache();
        }
    }
}