using Cinecritic.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinecritic.Infrastructure.Data
{
    public static class DbInitializer
    {
        public async static Task CreateInitialMoviesAsync(ApplicationDbContext context)
        {
            context.Database.Migrate();

            if (await context.Reviews.CountAsync() >= 50)
            {
                return;
            }
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Movies', RESEED, 0)");
            List<Movie> movies = new List<Movie>()
            {
                    new Movie
    {
        MovieTypeId = 1,
        Title = "Interstellar",
        ReleaseDate = new DateOnly(2014, 11, 7),
        Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Whiplash",
        ReleaseDate = new DateOnly(2014, 10, 10),
        Description = "A promising young drummer enrolls at a music conservatory where an abusive instructor pushes him to the limit."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Oppenheimer",
        ReleaseDate = new DateOnly(2023, 7, 21),
        Description = "The story of J. Robert Oppenheimer and the development of the atomic bomb."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "La La Land",
        ReleaseDate = new DateOnly(2016, 12, 9),
        Description = "A jazz musician and an aspiring actress fall in love while pursuing their dreams in Los Angeles."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Fight Club",
        ReleaseDate = new DateOnly(1999, 10, 15),
        Description = "An office worker and a soap maker form an underground fight club that evolves into something much more."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "The Dark Knight",
        ReleaseDate = new DateOnly(2008, 7, 18),
        Description = "Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos."
    },
    new Movie
    {
        MovieTypeId = 3,
        Title = "Spirited Away",
        ReleaseDate = new DateOnly(2001, 7, 20),
        Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches, and spirits."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Dead Poets Society",
        ReleaseDate = new DateOnly(1989, 6, 2),
        Description = "English teacher John Keating inspires his students through his teaching of poetry."
    },
    new Movie
    {
        MovieTypeId = 3,
        Title = "Inside Out 2",
        ReleaseDate = new DateOnly(2025, 6, 14),
        Description = "Riley's emotions return for a new adventure as she navigates adolescence."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Shutter Island",
        ReleaseDate = new DateOnly(2010, 2, 19),
        Description = "A U.S. Marshal investigates the disappearance of a murderer who escaped from a hospital for the criminally insane."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Superman",
        ReleaseDate = new DateOnly(1978, 12, 15),
        Description = "Clark Kent, an alien orphan, grows up to become the superhero Superman."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Guardians of the Galaxy",
        ReleaseDate = new DateOnly(2014, 8, 1),
        Description = "A group of intergalactic criminals must pull together to stop a fanatical warrior."
    },
    new Movie
    {
        MovieTypeId = 1,
        Title = "Avengers",
        ReleaseDate = new DateOnly(2012, 5, 4),
        Description = "Earth's mightiest heroes must come together to stop Loki and his alien army."
    },
            };
            for (int i = 0; i < 100; i++)
            {
                movies.Add(new Movie
                {
                    Title = $"TestMovie{i}",
                    MovieTypeId = 1,
                    ReleaseDate = DateOnly.FromDateTime(DateTime.UtcNow)
                });
            }
            context.Movies.AddRange(movies);
            List<ApplicationUser> users = new List<ApplicationUser>();

            for (int i = 0; i < 50; i++)
            {
                users.Add(new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"test{i}@gmail.com",
                    Email = $"test{i}@gmail.com",
                    DisplayName = $"test{i}"
                });
            }
            context.Users.AddRange(users);

            List<MovieUser> movieUsers = new List<MovieUser>();
            for (int i = 0; i < 50; i++)
            {
                movieUsers.Add(new MovieUser
                {
                    UserId = users[i].Id,
                    MovieId = 1,
                    Review = new Review
                    {
                        ReviewText = $"TestrReview{i}"
                    }
                });
            }
            context.MovieUsers.AddRange(movieUsers);

            await context.SaveChangesAsync();
        }
    }
}
