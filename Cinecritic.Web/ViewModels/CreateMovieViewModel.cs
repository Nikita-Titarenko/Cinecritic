using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Web.ViewModels
{
    public class CreateMovieViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }
    }
}