namespace Cinecritic.Application.DTOs.MovieUsers
{
    public class RateMovieDto
    {
        public int ApplicationUserId { get; set; }

        public int MovieId { get; set; }

        public int Rate { get; set; }
    }
}
