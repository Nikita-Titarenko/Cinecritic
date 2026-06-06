using System.ComponentModel.DataAnnotations;
using Cinecritic.Infrastructure.Data;

namespace Cinecritic.Domain.Models
{
    public class MovieUser
    {
        public int Id { get; set; }
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        public int ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = default!;

        public DateTime WatchedDateTime { get; set; } = DateTime.UtcNow;

        public bool IsLiked { get; set; }

        public DateTime? LikedDateTime { get; set; }

        [Range(1, 10)]
        public int? Rate { get; set; }

        public Review? Review { get; set; }
    }
}
