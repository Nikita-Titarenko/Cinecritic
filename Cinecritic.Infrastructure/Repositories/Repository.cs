using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using MongoDB.Bson;

namespace Cinecritic.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoDatabase _database;

        protected readonly IMongoCollection<T> _collection;

        public Repository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<T>($"{typeof(T).Name}s");
        }
        
        public async Task<T?> GetByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var id = (Guid)GetIdValue(entity);
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }
        
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            var updates = new List<WriteModel<T>>();

            foreach (var entity in entities)
            {
                var id = (Guid)GetIdValue(entity);
                var filter = Builders<T>.Filter.Eq("_id", id);
                
                updates.Add(new ReplaceOneModel<T>(filter, entity) 
                { 
                    IsUpsert = true 
                });
            }

            if (updates.Count != 0)
            {
                await _collection.BulkWriteAsync(updates);
            }
        }

        public async Task DeleteAsync(T entity)
        {
            var id = (Guid)GetIdValue(entity);
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<int> CountAsync()
        {
            var count = await _collection.CountDocumentsAsync(_ => true);
            return (int)count;
        }

        private object GetIdValue(T entity)
        {
            return entity.GetType().GetProperty("Id")?.GetValue(entity, null)
                   ?? entity.GetType().GetProperty("_id")?.GetValue(entity, null)!;
        }
    }
}