using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models;

public class GeoData
{
    [BsonElement("type")]
    public string Type { get; set; } = "Point";
    [BsonElement("coordinates")]
    public double[] Coordinates { get; set; } = [];
}