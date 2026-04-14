using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Domain.Models;
using FluentResults;
using MongoDB.Bson;

namespace Cinecritic.Application.Services.Movies;

public class MovieService(
    IMovieRepository movieRepository,
    IMapper mapper,
    IFileService fileService,
    IRepository<FilmingLocation> filmingLocationRepository) : IMovieService
{
    private readonly IMovieRepository _movieRepository = movieRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IFileService _fileService = fileService;
    private readonly IRepository<FilmingLocation> _filmingLocationRepository = filmingLocationRepository;
    private const string MoviePath = "movie-posters";

    public async Task<Result<ObjectId>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension)
    {
        var movie = _mapper.Map<Movie>(dto);

        if (stream != null)
        {
            var path = Path.Combine(MoviePath, $"{movie.Id}{fileExtension}");
            await _fileService.SaveFile(path, stream);
            movie.ImagePath = GetFilePath(movie.Id);
        }

        await _movieRepository.AddAsync(movie);
        dto.FilmingLocations.ForEach(loc => loc.MovieId = movie.Id);
        if (dto.FilmingLocations.Count > 0)
        {
            await _filmingLocationRepository.AddRangeAsync(dto.FilmingLocations);
        }

        return Result.Ok(movie.Id);
    }

    public async Task<Result> UpdateMovieAsync(ObjectId id, CreateMovieDto dto, Stream? stream, string? fileExtension)
    {
        var existingMovie = await _movieRepository.GetByIdAsync(id);
        if (existingMovie == null)
        {
            return Result.Fail("Movie not found");
        }

        _mapper.Map(dto, existingMovie);
        existingMovie.Id = id;

        if (stream != null)
        {
            var path = Path.Combine(MoviePath, $"{id}{fileExtension}");
            await _fileService.SaveFile(path, stream);
            existingMovie.ImagePath = GetFilePath(id);
        }

        await _movieRepository.UpdateAsync(existingMovie);
        dto.FilmingLocations.ForEach(loc => loc.MovieId = existingMovie.Id);
        await _filmingLocationRepository.UpdateRangeAsync(dto.FilmingLocations);
        return Result.Ok();
    }

    public async Task<Result> DeleteMovieAsync(ObjectId id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie == null)
        {
            return Result.Fail("Movie not found");
        }

        await _movieRepository.DeleteAsync(movie);
        return Result.Ok();
    }

    public async Task<Result<GetMoviesResultDto>> GetMoviesAsync(int pageSize, int pageCount)
    {
        var movies = await _movieRepository.GetMoviesAsync(pageSize, pageCount);

        return Result.Ok(new GetMoviesResultDto
        {
            Movies = movies,
            TotalMovieNumber = await _movieRepository.CountAsync()
        });
    }

    public async Task<Result<MovieWithReviewsDto>> GetMovieAsync(ObjectId movieId, ObjectId? userId, int reviewCount = 10)
    {
        var movie = await _movieRepository.GetMovieWithReviewsAsync(movieId, userId);

        return movie == null ? (Result<MovieWithReviewsDto>)Result.Fail(new Error("Movie not exist").WithMetadata("Code", "MovieNotExist")) : Result.Ok(movie);
    }

    public async Task<Result<IEnumerable<FilmingLocation>>> GetNearestLocationsAsync(double latitude, double longitude, int nPoints = 5)
    {
        var geoData = new GeoData
        {
            Type = "Point",
            Coordinates = [longitude, latitude]
        };

        var locations = await _movieRepository.GetNearestFilmingLocationsAsync(geoData, nPoints);
        return Result.Ok(locations);
    }

    private string GetFilePath(ObjectId movieId)
    {
        var result = _fileService.GetFilePath(Path.Combine(MoviePath, $"{movieId}.jpg"));
        return result.IsSuccess ? result.Value : "/images/no-image.webp";
    }
}