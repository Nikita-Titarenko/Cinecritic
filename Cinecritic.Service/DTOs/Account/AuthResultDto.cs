using MongoDB.Bson;

namespace Cinecritic.Application.DTOs.Account
{
    public class AuthResultDto
    {
        public ObjectId? UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
