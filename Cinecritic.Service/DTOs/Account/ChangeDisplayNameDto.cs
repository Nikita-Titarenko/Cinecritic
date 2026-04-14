using MongoDB.Bson;

namespace Cinecritic.Application.DTOs.Account;

public class ChangeDisplayNameDto
{
    public ObjectId UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}
