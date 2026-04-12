using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models
{
    public class MovieType
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
