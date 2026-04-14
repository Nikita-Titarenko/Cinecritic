using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models
{
    public class MovieType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
