using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models
{
    public class Movie
    {
        [BsonId]
        public Guid Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public string ImagePath { get; set; } = "/images/no-image.webp";

        public double AverageRating { get; set; }
        public int TotalWatches { get; set; }
        public int LikedCount { get; set; }
        public int WatchListCount { get; set; }
        
        public MovieType MovieType { get; set; } = null!;

        [BsonIgnoreIfDefault] 
        public List<FilmingLocation> FilmingLocations { get; set; } = [];
    }
}