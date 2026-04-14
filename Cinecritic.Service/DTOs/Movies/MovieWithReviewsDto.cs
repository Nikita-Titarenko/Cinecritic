using Cinecritic.Domain.Models;
using MongoDB.Bson;

namespace Cinecritic.Application.DTOs.Movies;

public class MovieWithReviewsDto
{
    public ObjectId Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string ImagePath { get; set; } = "/images/no-image.webp";

    public MovieType MovieType { get; set; } = new MovieType();

    public double AverageRating { get; set; }
    public int TotalWatches { get; set; }
    public int LikedCount { get; set; }
    public int WatchListCount { get; set; }

    public MovieUser CurrentUserInteraction { get; set; } = new();

    public List<MovieUser> Reviews { get; set; } = [];

    public List<MovieUser> CurrentUserInteractionItems { get; set; } = [];

    public List<FilmingLocation> FilmingLocations { get; set; } = [];
}