namespace Cinecritic.Application.DTOs.Movies
{
    public class MovieUserStatusDto
    {
        public string UserId { get; set; } = string.Empty;

        public bool IsWatched { get; set; }

        public bool IsLiked { get; set; }

        public bool IsInWatchList { get; set; }

        public int? Rate { get; set; }
    }
}
