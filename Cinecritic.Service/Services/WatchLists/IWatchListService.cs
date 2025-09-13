using Cinecritic.Application.DTOs.Movies;
using FluentResults;

namespace Cinecritic.Application.Services.WatchLists
{
    public interface IWatchListService
    {
        Task<Result<MovieUserStatusDto>> ToggleWatchListMovieAsync(int movieId, string userId);
    }
}