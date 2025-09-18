using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinecritic.Application.DTOs.MovieUsers
{
    public class MovieReviewDto
    {
        public string UserId { get; set; } = string.Empty;

        public int MovieId { get; set; }

        public string ReviewText { get; set; } = string.Empty;

        public DateOnly ReviewDate { get; set; }

        public bool IsLiked { get; set; }

        public int? Rate { get; set; }

        public string DisplayName { get; set; } = string.Empty;
    }
}
