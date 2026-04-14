using Cinecritic.Domain.Models;

namespace Cinecritic.Web.ViewModels.Movies;

public class MovieListViewModel
{
    public List<Movie> Movies { get; set; } = [];

    public int TotalPageNumber { get; set; }
}
