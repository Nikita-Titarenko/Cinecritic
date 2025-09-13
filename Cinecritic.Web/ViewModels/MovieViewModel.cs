namespace Cinecritic.Web.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateOnly? ReleaseDate { get; set; }

        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public MovieUserStatusViewModel MovieUserStatus { get; set; } = new MovieUserStatusViewModel();
    }
}
