using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Web.ViewModels.MovieUsers;
using Cinecritic.Web.ViewModels.Reviews;

namespace Cinecritic.Web.ViewModels.Movies
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly? ReleaseDate { get; set; }

        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public MovieUserStatusViewModel MovieUserStatus { get; set; } = new MovieUserStatusViewModel();

        public int WatchedCount { get; set; }

        public int LikedCount { get; set; }

        public int WatchListCount { get; set; }

        public double Rate { get; set; }

        public List<MovieReviewViewModel> Reviews { get; set; } = new List<MovieReviewViewModel>();
    }
}
