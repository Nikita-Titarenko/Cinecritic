using Cinecritic.Domain.Models;
using MongoDB.Bson;

namespace Cinecritic.Application.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task UpdateDisplayNameAsync(ObjectId userId, string newName);
}