namespace Cinecritic.Application.DTOs.Reviews
{
    public class UpsertMovieReviewDto
    {
        public string UserId { get; set; } = string.Empty;

        public int MovieId { get; set; }

        public string ReviewText { get; set; } = string.Empty;
    }
}
