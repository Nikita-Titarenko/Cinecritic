namespace Cinecritic.Web.ViewModels
{
    public class MovieListItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly? ReleaseDate { get; set; }

        public string? ImagePath { get; set; }
    }
}
