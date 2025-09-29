namespace Cinecritic.Domain.Models
{
    public class WatchList
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;

        public DateTime InWatchListDateTime { get; set; } = DateTime.UtcNow;
    }
}
