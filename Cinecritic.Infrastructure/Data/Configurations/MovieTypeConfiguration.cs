using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinecritic.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Cinecritic.Infrastructure.Data.Configurations
{
    public class MovieTypeConfiguration : IEntityTypeConfiguration<MovieType>
    {
        public void Configure(EntityTypeBuilder<MovieType> builder)
        {
            IEnumerable<MovieType> movieTypes = new List<MovieType> {
                new MovieType
                {
                    MovieTypeName = "Movie",
                },
                new MovieType
                {
                    MovieTypeName = "Series",
                },
                new MovieType
                {
                    MovieTypeName = "Cartoon",
                },
            };
            builder.HasData(movieTypes);
        }
    }
}
