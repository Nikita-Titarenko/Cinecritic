namespace Cinecritic.Application.DTOs.Reviews
{
    public class UpsertMovieReviewDto
    {
        public int ApplicationUserId { get; set; }

        public int MovieId { get; set; }

        public string ReviewText { get; set; } = string.Empty;
    }
}
