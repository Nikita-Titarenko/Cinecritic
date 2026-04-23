using Cinecritic.Application.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cinecritic.Infrastructure.Repositories;

public class Repository<T>(IMongoDatabase database) : IRepository<T> where T : class
{
    protected IMongoDatabase Database { get; set; } = database;

    protected IMongoCollection<T> Collection { get; set; } = database.GetCollection<T>($"{typeof(T).Name}s");

    public async Task<T?> GetByIdAsync(ObjectId id)
    {
        var filter = Builders<T>.Filter.Eq("Id", id);
        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        var id = GetIdValue(entity);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await Collection.ReplaceOneAsync(filter, entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await Collection.InsertManyAsync(entities);
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        var updates = new List<WriteModel<T>>();

        foreach (var entity in entities)
        {
            var id = GetIdValue(entity);
            var filter = Builders<T>.Filter.Eq("_id", id);

            updates.Add(new ReplaceOneModel<T>(filter, entity)
            {
                IsUpsert = true
            });
        }

        if (updates.Count != 0)
        {
            await Collection.BulkWriteAsync(updates);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        var id = GetIdValue(entity);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await Collection.DeleteOneAsync(filter);
    }

    public async Task<int> CountAsync()
    {
        var count = await Collection.CountDocumentsAsync(_ => true);
        return (int)count;
    }

    private static ObjectId GetIdValue(T entity)
    {
        return (ObjectId)(entity.GetType().GetProperty("Id")?.GetValue(entity, null)
               ?? entity.GetType().GetProperty("_id")?.GetValue(entity, null)!);
    }
}