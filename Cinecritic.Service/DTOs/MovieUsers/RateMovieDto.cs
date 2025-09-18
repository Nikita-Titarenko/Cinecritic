namespace Cinecritic.Application.DTOs.MovieUsers
{
    public class RateMovieDto
    {
        public string UserId { get; set; } = string.Empty;

        public int MovieId { get; set; }

        public int Rate { get; set; }
    }
}
