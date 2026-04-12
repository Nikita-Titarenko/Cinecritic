using Cinecritic.Domain.Models;

namespace Cinecritic.Application.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task UpdateDisplayNameAsync(Guid userId, string newName);
}