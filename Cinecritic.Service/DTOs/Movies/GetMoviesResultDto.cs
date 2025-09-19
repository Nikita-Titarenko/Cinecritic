namespace Cinecritic.Application.DTOs.Movies
{
    public class GetMoviesResultDto
    {
        public IEnumerable<MovieListItemDto> Movies { get; set; } = new List<MovieListItemDto>();

        public int TotalMovieNumber { get; set; }
    }
}
