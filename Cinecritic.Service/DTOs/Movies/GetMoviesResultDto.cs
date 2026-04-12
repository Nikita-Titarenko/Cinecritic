using Cinecritic.Domain.Models;

namespace Cinecritic.Application.DTOs.Movies
{
    public class GetMoviesResultDto
    {
        public IEnumerable<Movie> Movies { get; set; } = new List<Movie>();

        public int TotalMovieNumber { get; set; }
    }
}
