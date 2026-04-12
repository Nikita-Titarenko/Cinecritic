using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models
{
    public class MovieUser
    {
        [BsonId]
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public Guid UserId { get; set; }
        
        public bool IsWatched { get; set; }

        public DateTime? WatchedDateTime { get; set; }

        public bool IsLiked { get; set; }

        public DateTime? LikedDateTime { get; set; }
        
        public bool IsInWatchList { get; set; }
        
        public DateTime? InWatchListDateTime { get; set; }
        
        public int? Rate { get; set; }
        
        public string? ReviewText { get; set; }

        public DateTime? ReviewDateTime { get; set; }
        
        [BsonIgnoreIfDefault]
        public string Name { get; set; } = string.Empty;
    }
}
