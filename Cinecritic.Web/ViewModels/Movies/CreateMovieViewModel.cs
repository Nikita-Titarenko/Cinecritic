using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace Cinecritic.Web.ViewModels.Movies
{
    public class CreateMovieViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        [Required]
        public int? SelectedMovieTypeId { get; set; } = null;
    }
}