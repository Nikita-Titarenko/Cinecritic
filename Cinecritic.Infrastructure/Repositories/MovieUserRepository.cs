using Cinecritic.Application.DTOs.Movies;
using MongoDB.Driver;
using Cinecritic.Domain.Models;
using Cinecritic.Application.Repositories;
using MongoDB.Bson;

namespace Cinecritic.Infrastructure.Repositories
{
    public class MovieUserRepository : Repository<MovieUser>, IMovieUserRepository
    {
        public MovieUserRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<MovieUser?> GetMovieUserAsync(ObjectId movieId, ObjectId userId)
        {
            return await _collection.Find(mu => mu.MovieId == movieId && mu.UserId == userId)
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

            var result = await _collection.Aggregate(pipeline).FirstOrDefaultAsync();

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
                new BsonDocument("$match", new BsonDocument {
                    { "UserId", userId },
                    { "IsWatched", true }
                }),

                new BsonDocument("$skip", (pageCount - 1) * pageSize),
                new BsonDocument("$limit", pageSize),

                new BsonDocument("$lookup", new BsonDocument {
                    { "from", "Movies" },
                    { "localField", "MovieId" },
                    { "foreignField", "_id" },
                    { "as", "MovieDetails" }
                }),

                new BsonDocument("$unwind", "$MovieDetails"),

                new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$MovieDetails"))
            };

            return await _collection.Aggregate<Movie>(pipeline).ToListAsync();
        }

        public async Task<int> CountWatched(ObjectId userId)
        {
            return (int)await _collection.CountDocumentsAsync(mu => mu.UserId == userId && mu.IsWatched);
        }

        public async Task<IEnumerable<Movie>> GetLikedMoviesAsync(ObjectId userId, int pageSize, int pageCount)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument {
                    { "UserId", userId },
                    { "IsLiked", true }
                }),

                new BsonDocument("$skip", (pageCount - 1) * pageSize),
                new BsonDocument("$limit", pageSize),

                new BsonDocument("$lookup", new BsonDocument {
                    { "from", "Movies" },
                    { "localField", "MovieId" },
                    { "foreignField", "_id" },
                    { "as", "MovieDetails" }
                }),

                new BsonDocument("$unwind", "$MovieDetails"),

                new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$MovieDetails"))
            };

            return await _collection.Aggregate<Movie>(pipeline).ToListAsync();
        }
        
        public async Task<IEnumerable<Movie>> GetInWatchListMoviesAsync(ObjectId userId, int pageSize, int pageCount)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument {
                    { "UserId", userId },
                    { "IsInWatchList", true }
                }),

                new BsonDocument("$skip", (pageCount - 1) * pageSize),
                new BsonDocument("$limit", pageSize),

                new BsonDocument("$lookup", new BsonDocument {
                    { "from", "Movies" },
                    { "localField", "MovieId" },
                    { "foreignField", "_id" },
                    { "as", "MovieDetails" }
                }),

                new BsonDocument("$unwind", "$MovieDetails"),

                new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$MovieDetails"))
            };

            return await _collection.Aggregate<Movie>(pipeline).ToListAsync();
        }

        public async Task<int> CountLiked(ObjectId userId)
        {
            return (int)await _collection.CountDocumentsAsync(mu => mu.UserId == userId && mu.IsLiked);
        }
    }
}