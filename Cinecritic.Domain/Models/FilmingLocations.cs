using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models;

public class FilmingLocation : BaseEntity
{
    public ObjectId MovieId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public GeoData Location { get; set; }
    
    [BsonIgnoreIfDefault]
    public string MovieName { get; set; } = string.Empty;
    
    [BsonIgnoreIfDefault]
    public double Distance { get; set; }
    
    [BsonIgnoreIfDefault]
    public List<Movie> MovieDetails { get; set; }
}