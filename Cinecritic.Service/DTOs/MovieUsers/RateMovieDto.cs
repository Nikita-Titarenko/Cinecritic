using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinecritic.Application.DTOs.MovieUsers
{
    public class RateMovieDto
    {
        public string UserId { get; set; } = string.Empty;

        public int MovieId { get; set; }

        public int Rate { get; set; }
    }
}
