namespace Cinecritic.Application.DTOs.MovieUsers
{
    public class UpsertMovieReviewDto
    {
        public string UserId { get; set; } = string.Empty;

        public int MovieId { get; set; }

        public string ReviewText { get; set; } = string.Empty;
    }
}
