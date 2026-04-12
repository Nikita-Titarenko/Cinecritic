namespace Cinecritic.Application.DTOs.Movies;

public record MovieStatisticsDto(
    double AverageRating,
    int TotalWatches,
    int LikedCount,
    int WatchListCount
);