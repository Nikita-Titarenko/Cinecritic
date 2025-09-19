namespace Cinecritic.Web.ViewModels.Reviews
{
    public class MovieReviewViewModel
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
