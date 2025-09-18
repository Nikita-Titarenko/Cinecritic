using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.DTOs.Reviews;

namespace Cinecritic.Application.DTOs.Movies
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly? ReleaseDate { get; set; }

        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public MovieUserStatusDto MovieUserStatus { get; set; } = new MovieUserStatusDto();

        public int WatchedCount { get; set; }

        public int LikedCount { get; set; }

        public int WatchListCount { get; set; }

        public double Rate { get; set; }

        public IEnumerable<MovieReviewDto> Reviews { get; set; } = new List<MovieReviewDto>();
    }
}
