using Cinecritic.Domain.Models;
using MongoDB.Bson;

namespace Cinecritic.Application.DTOs.Movies;

public class CreateMovieDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateOnly? ReleaseDate { get; set; }

    public ObjectId MovieTypeId { get; set; }

    public string MovieTypeName { get; set; } = string.Empty;

    public List<FilmingLocation> FilmingLocations { get; set; } = [];
}
