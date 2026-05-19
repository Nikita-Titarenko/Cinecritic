namespace Cinecritic.Application.DTOs.Movies;

public class TopMovieQueryResult
{
    public int MovieId { get; set; }
    public string Title { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalWatches { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}