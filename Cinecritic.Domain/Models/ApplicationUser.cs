namespace Cinecritic.Domain.Models;

public class ApplicationUser : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
