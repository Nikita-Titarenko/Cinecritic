using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cinecritic.Infrastructure.Repositories;

public class UserRepository(IMongoDatabase database) : Repository<ApplicationUser>(database), IUserRepository
{
    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await Collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task UpdateDisplayNameAsync(ObjectId userId, string newName)
    {
        var filter = Builders<ApplicationUser>.Filter.Eq(u => u.Id, userId);
        var update = Builders<ApplicationUser>.Update.Set(u => u.Name, newName);
        await Collection.UpdateOneAsync(filter, update);
    }
}