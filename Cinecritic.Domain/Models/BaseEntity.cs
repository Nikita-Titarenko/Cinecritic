using MongoDB.Bson;

namespace Cinecritic.Domain.Models;

public class BaseEntity
{
    public ObjectId Id { get; set; }
}