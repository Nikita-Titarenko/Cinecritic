using System.ComponentModel.DataAnnotations;
using Cinecritic.Domain.Models;

namespace Cinecritic.Web.ViewModels.Movies;

public class CreateMovieViewModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    [Required]
    public string? SelectedMovieTypeId { get; set; } = null;

    public List<FilmingLocation> FilmingLocations { get; set; } = [];
}