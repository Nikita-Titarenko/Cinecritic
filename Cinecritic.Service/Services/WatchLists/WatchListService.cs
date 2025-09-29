using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.WatchLists
{
    public class WatchListService : IWatchListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public WatchListService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
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

        public async Task<Result<GetMoviesResultDto>> GetMoviesInWatchListAsync(string userId, int pageSize, int pageCount)
        {
            var movies = await _unitOfWork.WatchLists.GetMoviesInWatchListAsync(userId, pageSize, pageCount);
            foreach (var movie in movies)
            {
                movie.ImagePath = GetFilePath(movie.Id);
            }
            var dto = new GetMoviesResultDto
            {
                Movies = movies,
                TotalMovieNumber = await _unitOfWork.WatchLists.Count(userId)
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
