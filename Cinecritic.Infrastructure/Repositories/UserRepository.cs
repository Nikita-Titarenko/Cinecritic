using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using MongoDB.Driver;

namespace Cinecritic.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(IMongoDatabase database)
            : base(database)
        {
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task UpdateDisplayNameAsync(Guid userId, string newName)
        {
            var filter = Builders<ApplicationUser>.Filter.Eq(u => u.Id, userId);
            var update = Builders<ApplicationUser>.Update.Set(u => u.Name, newName);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}