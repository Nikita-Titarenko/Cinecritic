namespace Cinecritic.Web.ViewModels.Movies
{
    public class MovieListViewModel
    {
        public List<MovieListItemViewModel> Movies { get; set; } = new List<MovieListItemViewModel>();

        public int TotalPageNumber { get; set; }
    }
}
