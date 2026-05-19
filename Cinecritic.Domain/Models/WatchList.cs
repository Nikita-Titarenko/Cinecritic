using Cinecritic.Infrastructure.Data;

namespace Cinecritic.Domain.Models
{
    public class WatchList
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        public int ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; } = default!;

        public DateTime InWatchListDateTime { get; set; } = DateTime.UtcNow;
    }
}
