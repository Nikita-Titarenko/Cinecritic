using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Web.ViewModels
{
    public class CreateMovieReviewViewModel
    {
        [Required]
        public string ReviewText { get; set; } = string.Empty;
    }
}
