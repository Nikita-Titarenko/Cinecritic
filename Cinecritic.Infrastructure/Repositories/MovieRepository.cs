using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cinecritic.Infrastructure.Repositories;

public class MovieRepository(IMongoDatabase database) : Repository<Movie>(database), IMovieRepository
{
    public async Task<IEnumerable<Movie>> GetMoviesAsync(int pageSize, int pageCount)
    {
        return await Collection.Find(FilterDefinition<Movie>.Empty)
            .SortByDescending(m => m.ReleaseDate)
            .Skip((pageCount - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();
    }

    public async Task<MovieWithReviewsDto?> GetMovieWithReviewsAsync(ObjectId movieId, ObjectId? userId)
    {
        var aggregate = Collection.Aggregate()
            .Match(m => m.Id == movieId)
            .AppendStage<MovieWithReviewsDto>(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "FilmingLocations" },
                { "localField", "_id" },
                { "foreignField", "MovieId" },
                { "as", "FilmingLocations" }
            }))
            .AppendStage<MovieWithReviewsDto>(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "MovieUsers" },
                { "let", new BsonDocument("movie_id", "$_id") },
                {
                    "pipeline", new BsonArray
                    {
                        new BsonDocument("$match", new BsonDocument
                        {
                            {
                                "$expr", new BsonDocument("$and", new BsonArray
                                {
                                    new BsonDocument("$eq", new BsonArray { "$MovieId", "$$movie_id" }),
                                    new BsonDocument("$ne", new BsonArray { "$ReviewText", "" }),
                                    new BsonDocument("$ne", new BsonArray { "$ReviewText", BsonNull.Value })
                                })
                            }
                        }),
                        new BsonDocument("$lookup", new BsonDocument
                        {
                            { "from", "Users" },
                            { "localField", "UserId" },
                            { "foreignField", "_id" },
                            { "as", "UserDetails" }
                        }),
                        new BsonDocument("$addFields", new BsonDocument("Name",
                            new BsonDocument("$arrayElemAt", new BsonArray { "$UserDetails.Name", 0 })
                        ))
                    }
                },
                { "as", "Reviews" }
            }));

        aggregate = userId.HasValue
            ? aggregate.AppendStage<MovieWithReviewsDto>(new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "MovieUsers" },
                { "let", new BsonDocument("movie_id", "$_id") },
                {
                    "pipeline", new BsonArray
                    {
                        new BsonDocument("$match", new BsonDocument
                        {
                            {
                                "$expr", new BsonDocument("$and", new BsonArray
                                {
                                    new BsonDocument("$eq", new BsonArray { "$MovieId", "$$movie_id" }),
                                    new BsonDocument("$eq", new BsonArray { "$UserId", userId.Value })
                                })
                            }
                        })
                    }
                },
                { "as", "CurrentUserInteractionItems" }
            }))
            : aggregate.AppendStage<MovieWithReviewsDto>(new BsonDocument("$addFields",
                new BsonDocument("CurrentUserInteractionItems", new BsonArray())));

        return await aggregate
            .Project(mwr => new MovieWithReviewsDto
            {
                Id = mwr.Id,
                Title = mwr.Title,
                Description = mwr.Description,
                ReleaseDate = mwr.ReleaseDate,
                ImagePath = mwr.ImagePath,
                MovieType = mwr.MovieType,
                AverageRating = mwr.AverageRating,
                TotalWatches = mwr.TotalWatches,
                LikedCount = mwr.LikedCount,
                WatchListCount = mwr.WatchListCount,
                Reviews = mwr.Reviews.Select(r => new MovieUser
                {
                    Id = r.Id,
                    MovieId = r.MovieId,
                    UserId = r.UserId,
                    Rate = r.Rate,
                    ReviewText = r.ReviewText,
                    ReviewDateTime = r.ReviewDateTime,
                    IsLiked = r.IsLiked,
                    Name = r.Name
                }).ToList(),
                CurrentUserInteraction = mwr.CurrentUserInteractionItems.FirstOrDefault(),
                FilmingLocations = mwr.FilmingLocations
            })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateMovieStatsAsync(ObjectId movieId, MovieStatisticsDto stats)
    {
        var filter = Builders<Movie>.Filter.Eq(m => m.Id, movieId);
        var update = Builders<Movie>.Update
            .Set(m => m.AverageRating, stats.AverageRating)
            .Set(m => m.TotalWatches, stats.TotalWatches)
            .Set(m => m.LikedCount, stats.LikedCount)
            .Set(m => m.WatchListCount, stats.WatchListCount);

        await Collection.UpdateOneAsync(filter, update);
    }

    public async Task<IEnumerable<FilmingLocation>> GetNearestFilmingLocationsAsync(GeoData myLocation, int nPoints)
    {
        var filmingLocationCollection = Database.GetCollection<FilmingLocation>("FilmingLocations");

        var pipeline = new List<BsonDocument>
        {
            new("$geoNear", new BsonDocument
            {
                {
                    "near",
                    new BsonDocument
                        { { "type", "Point" }, { "coordinates", new BsonArray(myLocation.Coordinates) } }
                },
                { "distanceField", "Distance" },
                { "spherical", true }
            }),
            new("$limit", nPoints),
            new("$lookup", new BsonDocument
            {
                { "from", "Movies" },
                { "localField", "MovieId" },
                { "foreignField", "_id" },
                { "as", "MovieDetails" }
            }),
            new("$addFields", new BsonDocument("MovieName",
                new BsonDocument("$arrayElemAt", new BsonArray { "$MovieDetails.Title", 0 })
            ))
        };

        return await filmingLocationCollection.Aggregate<FilmingLocation>(pipeline).ToListAsync();
    }
}