namespace Cinecritic.Web.ViewModels.Reviews
{
    public class MovieReviewViewModel
    {
        public int ApplicationUserId { get; set; }

        public int MovieId { get; set; }

        public string ReviewText { get; set; } = string.Empty;

        public DateOnly ReviewDate { get; set; }

        public bool IsLiked { get; set; }

        public int? Rate { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public bool IsCenturion { get; set; }
    }
}
