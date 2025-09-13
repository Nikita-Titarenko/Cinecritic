using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
