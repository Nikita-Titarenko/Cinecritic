using Cinecritic.Domain.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Cinecritic.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task CreateInitialMoviesAsync(IMongoDatabase database)
        {
            var movieCollection = database.GetCollection<Movie>("Movies");
            var userCollection = database.GetCollection<ApplicationUser>("Users");
            var movieUserCollection = database.GetCollection<MovieUser>("MovieUsers");
            var movieTypeCollection = database.GetCollection<MovieType>("MovieTypes");
            var filmingLocationCollection = database.GetCollection<FilmingLocation>("FilmingLocations");

            var movieCount = await movieCollection.CountDocumentsAsync(_ => true);
            if (movieCount >= 10) return;

            await database.DropCollectionAsync("Movies");
            await database.DropCollectionAsync("Users");
            await database.DropCollectionAsync("MovieUsers");
            await database.DropCollectionAsync("MovieTypes");
            await database.DropCollectionAsync("Roles");
            await database.DropCollectionAsync("Reviews");

            var movieTypes = new List<MovieType>
            {
                new MovieType { Id = Guid.NewGuid(), Name = "Movie" },
                new MovieType { Id = Guid.NewGuid(), Name = "Series" },
                new MovieType { Id = Guid.NewGuid(), Name = "Cartoon" }
            };
            await movieTypeCollection.InsertManyAsync(movieTypes);

            var typeMovie = new MovieType { Id = movieTypes[0].Id, Name = movieTypes[0].Name };
            var typeSeries = new MovieType { Id = movieTypes[1].Id, Name = movieTypes[1].Name };
            var typeCartoon = new MovieType { Id = movieTypes[2].Id, Name = movieTypes[2].Name };

            var movies = new List<Movie>()
{
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Interstellar",
        ReleaseDate = new DateOnly(2014, 11, 7),
        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/1.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Whiplash",
        ReleaseDate = new DateOnly(2014, 10, 10),
        Description = "A promising young drummer enrolls at a music conservatory where an abusive instructor pushes him to the limit.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/2.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Oppenheimer",
        ReleaseDate = new DateOnly(2023, 7, 21),
        Description = "The story of J. Robert Oppenheimer and the development of the atomic bomb.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/3.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "La La Land",
        ReleaseDate = new DateOnly(2016, 12, 9),
        Description = "A jazz musician and an aspiring actress fall in love while pursuing their dreams in Los Angeles.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/4.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Fight Club",
        ReleaseDate = new DateOnly(1999, 10, 15),
        Description = "An office worker and a soap maker form an underground fight club that evolves into something much more.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/5.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "The Dark Knight",
        ReleaseDate = new DateOnly(2008, 7, 18),
        Description = "Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/6.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeCartoon,
        Title = "Spirited Away",
        ReleaseDate = new DateOnly(2001, 7, 20),
        Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches, and spirits.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/7.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Dead Poets Society",
        ReleaseDate = new DateOnly(1989, 6, 2),
        Description = "English teacher John Keating inspires his students through his teaching of poetry.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/8.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeCartoon,
        Title = "Inside Out 2",
        ReleaseDate = new DateOnly(2025, 6, 14),
        Description = "Riley's emotions return for a new adventure as she navigates adolescence.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/9.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Shutter Island",
        ReleaseDate = new DateOnly(2010, 2, 19),
        Description = "A U.S. Marshal investigates the disappearance of a murderer who escaped from a hospital for the criminally insane.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/10.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Superman",
        ReleaseDate = new DateOnly(1978, 12, 15),
        Description = "Clark Kent, an alien orphan, grows up to become the superhero Superman.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/11.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Guardians of the Galaxy",
        ReleaseDate = new DateOnly(2014, 8, 1),
        Description = "A group of intergalactic criminals must pull together to stop a fanatical warrior.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/12.jpg"
    },
    new Movie
    {
        Id = Guid.NewGuid(),
        MovieType = typeMovie,
        Title = "Avengers",
        ReleaseDate = new DateOnly(2012, 5, 4),
        Description = "Earth's mightiest heroes must come together to stop Loki and his alien army.",
        ImagePath = "https://localhost:44351/uploads/movie-posters/13.jpg"
    }
};

            await movieCollection.InsertManyAsync(movies);
            var locations = new List<FilmingLocation>
{
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[0].Id,
        PlaceName = "Eldhraun, Iceland",
        Description = "Used for the ice planet Miller scenes.",
        Location = new GeoData { Type = "Point", Coordinates = [-18.1101, 63.6755] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[0].Id,
        PlaceName = "Fort Macleod, Alberta",
        Description = "The town used for the dust storm and farm scenes.",
        Location = new GeoData { Type = "Point", Coordinates = [-113.4000, 49.7200] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[1].Id,
        PlaceName = "Palace Theatre, Los Angeles",
        Description = "The location for the final JVC Jazz Festival competition.",
        Location = new GeoData { Type = "Point", Coordinates = [-118.2501, 34.0435] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[2].Id,
        PlaceName = "Los Alamos Laboratory, New Mexico",
        Description = "The main laboratory site for the Manhattan Project.",
        Location = new GeoData { Type = "Point", Coordinates = [-106.2983, 35.8811] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[2].Id,
        PlaceName = "Institute for Advanced Study, Princeton",
        Description = "Where Oppenheimer met with Albert Einstein.",
        Location = new GeoData { Type = "Point", Coordinates = [-74.6617, 40.3323] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[3].Id,
        PlaceName = "Griffith Observatory, LA",
        Description = "Location of the famous planetarium dance scene.",
        Location = new GeoData { Type = "Point", Coordinates = [-118.3003, 34.1184] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[3].Id,
        PlaceName = "Hermosa Beach Pier",
        Description = "Where Sebastian sings 'City of Stars'.",
        Location = new GeoData { Type = "Point", Coordinates = [-118.4021, 33.8611] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[4].Id,
        PlaceName = "Promenade Towers, Los Angeles",
        Description = "The Narrator's apartment building that explodes.",
        Location = new GeoData { Type = "Point", Coordinates = [-118.2530, 34.0560] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[5].Id,
        PlaceName = "Old Post Office, Chicago",
        Description = "The exterior of the Gotham City National Bank (opening robbery).",
        Location = new GeoData { Type = "Point", Coordinates = [-87.6394, 41.8732] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[7].Id,
        PlaceName = "St. Andrew's School, Delaware",
        Description = "The real-life boarding school used as Welton Academy.",
        Location = new GeoData { Type = "Point", Coordinates = [-75.6836, 39.4286] }
    },
    new FilmingLocation
    {
        Id = Guid.NewGuid(),
        MovieId = movies[9].Id,
        PlaceName = "Medfield State Hospital, Massachusetts",
        Description = "The asylum used for the interior and exterior scenes.",
        Location = new GeoData { Type = "Point", Coordinates = [-71.2917, 42.1969] }
    }
};

await filmingLocationCollection.InsertManyAsync(locations);

            var users = new List<ApplicationUser>();
            for (int i = 0; i < 50; i++)
            {
                users.Add(new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = $"test{i}@gmail.com",
                    Name = $"test{i}"
                });
            }
            await userCollection.InsertManyAsync(users);

            var movieUsers = new List<MovieUser>();
            var firstMovieId = movies[0].Id;

            for (int i = 0; i < 50; i++)
            {
                var user = users[i];

                movieUsers.Add(new MovieUser
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    MovieId = firstMovieId,
                    Rate = 10,
                });
            }

            await movieUserCollection.InsertManyAsync(movieUsers);

            var update = Builders<Movie>.Update
                .Set(m => m.AverageRating, 10.0)
                .Set(m => m.TotalWatches, 50);
            await movieCollection.UpdateOneAsync(m => m.Id == firstMovieId, update);
            
            var indexKeysDefinition = Builders<FilmingLocation>.IndexKeys.Geo2DSphere(x => x.Location);
            
            var indexModel = new CreateIndexModel<FilmingLocation>(indexKeysDefinition);
            
            await filmingLocationCollection.Indexes.CreateOneAsync(indexModel);
        }
    }
}