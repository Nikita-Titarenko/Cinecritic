using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Domain.Models
{
    public class MovieUser
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;

        public DateTime WatchedDateTime {  get; set; } = DateTime.UtcNow;

        public bool IsLiked { get; set; }

        public DateTime? LikedDateTime { get; set; }

        [Range(1, 10)]
        public int? Rate { get; set; }

        public Review? Review { get; set; }
    }
}
