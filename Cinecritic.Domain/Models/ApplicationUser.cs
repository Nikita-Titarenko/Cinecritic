using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Cinecritic.Domain.Models
{
    public class ApplicationUser
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        
        public string Name { get; set; } = string.Empty;
    }
}
