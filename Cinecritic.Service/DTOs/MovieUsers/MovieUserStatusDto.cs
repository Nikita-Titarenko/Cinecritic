namespace Cinecritic.Application.DTOs.MovieUsers
{
    public class MovieUserStatusDto
    {
        public Guid UserId { get; set; }

        public bool IsWatched { get; set; }

        public bool IsLiked { get; set; }

        public bool IsInWatchList { get; set; }

        public int? Rate { get; set; }

        public string? ReviewText { get; set; }

        public DateOnly? ReviewDate { get; set; }
    }
}
