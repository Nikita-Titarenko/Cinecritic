using Cinecritic.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinecritic.Infrastructure.Data.Configurations
{
    public class MovieTypeConfiguration : IEntityTypeConfiguration<MovieType>
    {
        public void Configure(EntityTypeBuilder<MovieType> builder)
        {
            IEnumerable<MovieType> movieTypes = new List<MovieType> {
                new MovieType
                {
                    Id = 1,
                    MovieTypeName = "Movie",
                },
                new MovieType
                {
                    Id = 2,
                    MovieTypeName = "Series",
                },
                new MovieType
                {
                    Id = 3,
                    MovieTypeName = "Cartoon",
                },
            };
            builder.HasData(movieTypes);
        }
    }
}
