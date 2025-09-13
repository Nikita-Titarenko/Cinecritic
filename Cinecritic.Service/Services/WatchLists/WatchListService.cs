using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.WatchLists
{
    public class WatchListService : IWatchListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WatchListService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MovieUserStatusDto>> ToggleWatchListMovieAsync(int movieId, string userId)
        {
            bool isInWatchList = false;
            var repo = _unitOfWork.Repository<WatchList>();
            var watchList = await repo.GetAsync(movieId, userId);

            if (watchList == null)
            {
                await DeleteFromMovieUserAsync(movieId, userId);
                repo.Add(new WatchList { MovieId = movieId, UserId = userId });
                isInWatchList = true;
            } else
            {
                repo.Delete(watchList);
            }

            await _unitOfWork.CommitAsync();

            return Result.Ok(new MovieUserStatusDto
            {
                UserId = userId,
                IsInWatchList = isInWatchList
            });
        }

        private async Task DeleteFromMovieUserAsync(int movieId, string userId)
        {
            var movieUserRepository = _unitOfWork.Repository<MovieUser>();
            var movieUser = await movieUserRepository.GetAsync(movieId, userId);
            if (movieUser != null)
            {
                movieUserRepository.Delete(movieUser);
            }
        }
    }
}
