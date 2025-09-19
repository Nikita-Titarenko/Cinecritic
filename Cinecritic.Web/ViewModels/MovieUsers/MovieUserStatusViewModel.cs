using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Web.ViewModels.MovieUsers
{
    public class MovieUserStatusViewModel
    {
        public string UserId { get; set; } = string.Empty;

        public bool IsWatched { get; set; }

        public bool IsLiked { get; set; }

        public bool IsInWatchList { get; set; }

        public int? Rate { get; set; }

        public bool IsReviewCreated { get; set; }
        [Required(ErrorMessage = "Text is requiered")]
        public string ReviewText { get; set; } = string.Empty;

        public DateOnly? ReviewDate { get; set; }
    }
}
