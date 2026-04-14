using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;
using MongoDB.Bson;

namespace Cinecritic.Application.Services.MovieUsers
{
    public class MovieUserService : IMovieUserService
    {
        private readonly IMovieUserRepository _movieUserRepository;
        private readonly IMovieRepository _movieRepository;

        public MovieUserService(
            IMovieUserRepository movieUserRepository,
            IMovieRepository movieRepository)
        {
            _movieUserRepository = movieUserRepository;
            _movieRepository = movieRepository;
        }

        public async Task<Result<MovieUser>> RateMovieAsync(ObjectId movieId, ObjectId userId, int rating)
        {
            var movieUser = await _movieUserRepository.GetMovieUserAsync(movieId, userId);

            if (movieUser == null)
            {                
                movieUser = new MovieUser
                {
                    MovieId = movieId, 
                    UserId = userId, 
                    Rate = rating,
                    IsWatched = true,
                    WatchedDateTime = DateTime.UtcNow
                };
                await _movieUserRepository.AddAsync(movieUser);
            }
            else
            {
                movieUser.Rate = rating;
                await _movieUserRepository.UpdateAsync(movieUser);
            }

            await SyncMovieStatisticsAsync(movieId);
            
            return Result.Ok(movieUser);
        }

        public async Task<Result<MovieUser>> ToggleWatchMovieAsync(ObjectId movieId, ObjectId userId)
        {
            var movieUser = await _movieUserRepository.GetMovieUserAsync(movieId, userId);

            if (movieUser == null)
            {
                movieUser = new MovieUser { MovieId = movieId, UserId = userId, IsWatched = true, WatchedDateTime = DateTime.UtcNow };
                await _movieUserRepository.AddAsync(movieUser);
            }
            else if (!movieUser.IsWatched)
            {
                movieUser.IsWatched = true;
                movieUser.WatchedDateTime = DateTime.UtcNow;
                movieUser.IsInWatchList = false;
                await _movieUserRepository.UpdateAsync(movieUser);
            } 
            else
            {
                await _movieUserRepository.DeleteAsync(movieUser);
                movieUser = new MovieUser{ MovieId = movieId, UserId = userId };
            }

            await SyncMovieStatisticsAsync(movieId);

            return Result.Ok(movieUser);
        }

        public async Task<Result<MovieUser>> ToggleLikeMovieAsync(ObjectId movieId, ObjectId userId)
        {
            var movieUser = await _movieUserRepository.GetMovieUserAsync(movieId, userId);

            if (movieUser == null)
            {
                movieUser = new MovieUser
                {
                    MovieId = movieId, 
                    UserId = userId, 
                    IsWatched = true, 
                    WatchedDateTime = DateTime.UtcNow, 
                    IsLiked = true, 
                    LikedDateTime = DateTime.UtcNow,
                    IsInWatchList = false
                };
                await _movieUserRepository.AddAsync(movieUser);
            }
            else
            {
                movieUser.IsLiked = !movieUser.IsLiked;
                movieUser.LikedDateTime = movieUser.IsLiked ? DateTime.UtcNow : null;
                if (movieUser.IsLiked)
                {
                    movieUser.IsWatched = true;
                    movieUser.IsInWatchList = false;
                }
                await _movieUserRepository.UpdateAsync(movieUser);
            }

            await SyncMovieStatisticsAsync(movieId);

            return movieUser;
        }
        
        public async Task<Result<MovieUser>> ToggleIsInWatchListAsync(ObjectId movieId, ObjectId userId)
        {
            var movieUser = await _movieUserRepository.GetMovieUserAsync(movieId, userId);

            if (movieUser == null)
            {
                movieUser = new MovieUser { MovieId = movieId, UserId = userId, IsInWatchList = true, InWatchListDateTime = DateTime.UtcNow };
                await _movieUserRepository.AddAsync(movieUser);
            }
            else
            {
                movieUser.IsInWatchList = !movieUser.IsInWatchList;
                movieUser.InWatchListDateTime = DateTime.UtcNow;
                
                movieUser.IsWatched = false;
                movieUser.IsLiked = false;
                movieUser.ReviewText = null;
                movieUser.Rate = null;
                
                await _movieUserRepository.UpdateAsync(movieUser);
            }

            await SyncMovieStatisticsAsync(movieId);

            return movieUser;
        }
        
        public async Task<Result<MovieUser>> CreateOrUpdateReviewAsync(ObjectId movieId, ObjectId userId, string reviewText)
        {
            var movieUser = await _movieUserRepository.GetMovieUserAsync(movieId, userId);

            if (movieUser == null)
            {
                movieUser = new MovieUser
                {
                    MovieId = movieId, 
                    UserId = userId, 
                    IsWatched = true, 
                    WatchedDateTime = DateTime.UtcNow,
                    ReviewText = reviewText,
                    ReviewDateTime = DateTime.UtcNow
                };
                await _movieUserRepository.AddAsync(movieUser);
            }
            else
            {
                movieUser.IsInWatchList = false;
                if (!movieUser.IsWatched)
                {
                    movieUser.IsWatched = true;
                    movieUser.WatchedDateTime = DateTime.UtcNow;
                }

                if (movieUser.IsInWatchList)
                {
                    movieUser.IsInWatchList = false;
                }
                
                movieUser.ReviewText = reviewText;
                movieUser.ReviewDateTime = DateTime.UtcNow;
                
                await _movieUserRepository.UpdateAsync(movieUser);
            }

            await SyncMovieStatisticsAsync(movieId);

            return movieUser;
        }

        public async Task<Result<GetMoviesResultDto>> GetWatchedMoviesAsync(ObjectId userId, int pageSize, int pageCount)
        {
            var movies = await _movieUserRepository.GetWatchedMoviesAsync(userId, pageSize, pageCount);

            var dto = new GetMoviesResultDto
            {
                Movies = movies,
                TotalMovieNumber = await _movieUserRepository.CountWatched(userId)
            };
            return Result.Ok(dto);
        }

        public async Task<Result<GetMoviesResultDto>> GetLikedMoviesAsync(ObjectId userId, int pageSize, int pageCount)
        {
            var movies = await _movieUserRepository.GetLikedMoviesAsync(userId, pageSize, pageCount);

            var dto = new GetMoviesResultDto
            {
                Movies = movies,
                TotalMovieNumber = await _movieUserRepository.CountLiked(userId)
            };
            return Result.Ok(dto);
        }
        
        public async Task<Result<GetMoviesResultDto>> GetInWatchListMoviesAsync(ObjectId userId, int pageSize, int pageCount)
        {
            var movies = await _movieUserRepository.GetInWatchListMoviesAsync(userId, pageSize, pageCount);

            var dto = new GetMoviesResultDto
            {
                Movies = movies,
                TotalMovieNumber = await _movieUserRepository.CountLiked(userId)
            };
            return Result.Ok(dto);
        }

        private async Task SyncMovieStatisticsAsync(ObjectId movieId)
        {
            var stats = await _movieUserRepository.GetMovieStatisticsAsync(movieId);

            await _movieRepository.UpdateMovieStatsAsync(movieId, stats);
        }
    }
}