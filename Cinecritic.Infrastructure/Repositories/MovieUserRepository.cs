using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cinecritic.Infrastructure.Repositories;

public class MovieUserRepository(IMongoDatabase database) : Repository<MovieUser>(database), IMovieUserRepository
{
    public async Task<MovieUser?> GetMovieUserAsync(ObjectId movieId, ObjectId userId)
    {
        return await Collection.Find(mu => mu.MovieId == movieId && mu.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<MovieStatisticsDto> GetMovieStatisticsAsync(ObjectId movieId)
    {
        var pipeline = new EmptyPipelineDefinition<MovieUser>()
            .Match(mu => mu.MovieId == movieId)
            .Group(mu => mu.MovieId, g => new
            {
                AverageRating = g.Where(mu => mu.Rate != null).Average(mu => mu.Rate),
                TotalWatches = g.Count(mu => mu.IsWatched),
                LikedCount = g.Count(mu => mu.IsLiked),
                WatchListCount = g.Count(mu => mu.IsInWatchList)
            });

        var result = await Collection.Aggregate(pipeline).FirstOrDefaultAsync();

        return result != null
            ? new MovieStatisticsDto(
                result.AverageRating ?? 0,
                result.TotalWatches,
                result.LikedCount,
                result.WatchListCount)
            : new MovieStatisticsDto(0, 0, 0, 0);
    }

    public async Task<IEnumerable<Movie>> GetWatchedMoviesAsync(ObjectId userId, int pageSize, int pageCount)
    {
        var pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument {
                { "UserId", userId },
                { "IsWatched", true }
            }),

            new("$skip", (pageCount - 1) * pageSize),
            new("$limit", pageSize),

            new("$lookup", new BsonDocument {
                { "from", "Movies" },
                { "localField", "MovieId" },
                { "foreignField", "_id" },
                { "as", "MovieDetails" }
            }),

            new("$unwind", "$MovieDetails"),

            new("$replaceRoot", new BsonDocument("newRoot", "$MovieDetails"))
        };

        return await Collection.Aggregate<Movie>(pipeline).ToListAsync();
    }

    public async Task<int> CountWatched(ObjectId userId)
    {
        return (int)await Collection.CountDocumentsAsync(mu => mu.UserId == userId && mu.IsWatched);
    }

    public async Task<IEnumerable<Movie>> GetLikedMoviesAsync(ObjectId userId, int pageSize, int pageCount)
    {
        var pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument {
                { "UserId", userId },
                { "IsLiked", true }
            }),

            new("$skip", (pageCount - 1) * pageSize),
            new("$limit", pageSize),

            new("$lookup", new BsonDocument {
                { "from", "Movies" },
                { "localField", "MovieId" },
                { "foreignField", "_id" },
                { "as", "MovieDetails" }
            }),

            new("$unwind", "$MovieDetails"),

            new("$replaceRoot", new BsonDocument("newRoot", "$MovieDetails"))
        };

        return await Collection.Aggregate<Movie>(pipeline).ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetInWatchListMoviesAsync(ObjectId userId, int pageSize, int pageCount)
    {
        var pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument {
                { "UserId", userId },
                { "IsInWatchList", true }
            }),

            new("$skip", (pageCount - 1) * pageSize),
            new("$limit", pageSize),

            new("$lookup", new BsonDocument {
                { "from", "Movies" },
                { "localField", "MovieId" },
                { "foreignField", "_id" },
                { "as", "MovieDetails" }
            }),

            new("$unwind", "$MovieDetails"),

            new("$replaceRoot", new BsonDocument("newRoot", "$MovieDetails"))
        };

        return await Collection.Aggregate<Movie>(pipeline).ToListAsync();
    }

    public async Task<int> CountLiked(ObjectId userId)
    {
        return (int)await Collection.CountDocumentsAsync(mu => mu.UserId == userId && mu.IsLiked);
    }
}