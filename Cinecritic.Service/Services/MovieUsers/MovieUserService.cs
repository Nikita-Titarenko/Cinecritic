using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieUsers
{
    public class MovieUserService : IMovieUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public MovieUserService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Result<MovieUserStatusDto>> RateMovieAsync(RateMovieDto dto)
        {
            var repo = _unitOfWork.MovieUsers;
            var movieUser = await repo.GetMovieUserWithReview(dto.MovieId, dto.UserId);

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(dto.MovieId, dto.UserId);
                movieUser = _mapper.Map<MovieUser>(dto);
                repo.Add(movieUser);

            }
            else
            {
                movieUser.Rate = dto.Rate;
            }

            await _unitOfWork.CommitAsync();
            var resultDto = _mapper.Map<MovieUserStatusDto>(movieUser);
            resultDto.IsWatched = true;
            return Result.Ok(resultDto);
        }

        public async Task<Result<MovieUserStatusDto>> ToggleWatchMovieAsync(int movieId, string userId)
        {
            var repo = _unitOfWork.MovieUsers;
            var movieUser = await repo.GetMovieUserWithReview(movieId, userId);
            bool isWatched = false;

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(movieId, userId);
                repo.Add(new MovieUser { MovieId = movieId, UserId = userId });
                isWatched = true;
            }
            else
            {
                repo.Delete(movieUser);
            }

            await _unitOfWork.CommitAsync();

            return Result.Ok(new MovieUserStatusDto
            {
                UserId = userId,
                IsWatched = isWatched
            });
        }

        public async Task<Result<MovieUserStatusDto>> ToggleLikeMovieAsync(int movieId, string userId)
        {
            var repo = _unitOfWork.MovieUsers;
            var movieUser = await repo.GetMovieUserWithReview(movieId, userId);
            bool isLiked = false;

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(movieId, userId);
                repo.Add(new MovieUser { MovieId = movieId, UserId = userId, IsLiked = true, LikedDateTime = DateTime.UtcNow });
                isLiked = true;
            }
            else
            {
                movieUser.IsLiked = !movieUser.IsLiked;
                isLiked = movieUser.IsLiked;
                if (isLiked)
                {
                    movieUser.LikedDateTime = DateTime.UtcNow;
                }
            }

            await _unitOfWork.CommitAsync();

            return Result.Ok(new MovieUserStatusDto
            {
                UserId = userId,
                IsWatched = true,
                IsLiked = isLiked,
                Rate = movieUser?.Rate,
                ReviewText = movieUser?.Review?.ReviewText,
                ReviewDate = movieUser?.Review?.ReviewDateTime.Date != null ? DateOnly.FromDateTime(movieUser.Review.ReviewDateTime.Date) : null
            });
        }

        public async Task DeleteFromWatchListAsync(int movieId, string userId)
        {
            var watchListRepository = _unitOfWork.Repository<WatchList>();
            var watchList = await watchListRepository.GetAsync(movieId, userId);
            if (watchList != null)
            {
                watchListRepository.Delete(watchList);
            }
        }

        public async Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(string userId, int pageSize, int pageCount)
        {
            var movies = await _unitOfWork.MovieUsers.GetWatchedMoviesAsync(userId, pageSize, pageCount);
            foreach (var movie in movies)
            {
                movie.ImagePath = GetFilePath(movie.Id);
            }
            var dto = new GetMoviesResultDto{
                Movies = movies,
                TotalMovieNumber = await _unitOfWork.MovieUsers.CountWatched(userId)
            };
            return Result.Ok(dto);
        }

        public async Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(string userId, int pageSize, int pageCount)
        {
            var movies = await _unitOfWork.MovieUsers.GetLikedMoviesAsync(userId, pageSize, pageCount);
            foreach (var movie in movies)
            {
                movie.ImagePath = GetFilePath(movie.Id);
            }
            var dto = new GetMoviesResultDto
            {
                Movies = movies,
                TotalMovieNumber = await _unitOfWork.MovieUsers.CountLiked(userId)
            };
            return Result.Ok(dto);
        }

        private string GetFilePath(int movieId)
        {
            var result = _fileService.GetFilePath(Path.Combine(MovieService.MoviePath, $"{movieId}.jpg"));
            return result.IsSuccess ? result.Value : "/images/no-image.webp";
        }
    }
}
