using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieUsers
{
    public class MovieUserService : IMovieUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieUserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MovieUserStatusDto>> RateMovieAsync(RateMovieDto dto)
        {
            var movieUserRepository = _unitOfWork.Repository<MovieUser>();
            var movieUser = await movieUserRepository.GetAsync(dto.MovieId, dto.UserId);

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(dto.MovieId, dto.UserId);
                movieUser = _mapper.Map<MovieUser>(dto);
                movieUserRepository.Add(movieUser);

            } else
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
            var repo = _unitOfWork.Repository<MovieUser>();
            var movieUser = await repo.GetAsync(movieId, userId);
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
            var repo = _unitOfWork.Repository<MovieUser>();
            var movieUser = await repo.GetAsync(movieId, userId);
            bool isLiked = false;

            if (movieUser == null)
            {
                await DeleteFromWatchListAsync(movieId, userId);
                repo.Add(new MovieUser { MovieId = movieId, UserId = userId, IsLiked = true });
                isLiked = true;
            }
            else
            {
                movieUser.IsLiked = !movieUser.IsLiked;
                isLiked = movieUser.IsLiked;
            }

            await _unitOfWork.CommitAsync();

            return Result.Ok(new MovieUserStatusDto
            {
                UserId = userId,
                IsWatched = true,
                IsLiked = isLiked,
                Rate = movieUser?.Rate
            });
        }

        private async Task DeleteFromWatchListAsync(int movieId, string userId)
        {
            var watchListRepository = _unitOfWork.Repository<WatchList>();
            var watchList = await watchListRepository.GetAsync(movieId, userId);
            if (watchList != null)
            {
                watchListRepository.Delete(watchList);
            }
        }
    }
}
