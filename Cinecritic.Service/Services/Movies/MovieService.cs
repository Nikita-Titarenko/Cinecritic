using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IRepository<FilmingLocation> _filmingLocationRepository;
        private const string MoviePath = "movie-posters";

        public MovieService(
            IMovieRepository movieRepository,
            IMapper mapper,
            IFileService fileService,
            IRepository<FilmingLocation> filmingLocationRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _fileService = fileService;
            _filmingLocationRepository = filmingLocationRepository;
        }

        public async Task<Result<Guid>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension)
        {
            var movie = _mapper.Map<Movie>(dto);
            
            movie.Id = Guid.NewGuid();

            if (stream != null)
            {
                var path = Path.Combine(MoviePath, $"{movie.Id}{fileExtension}");
                await _fileService.SaveFile(path, stream);
                movie.ImagePath = GetFilePath(movie.Id);
            }
            
            await _movieRepository.AddAsync(movie);
            dto.FilmingLocations.ForEach(loc => loc.MovieId = movie.Id);
            await _filmingLocationRepository.AddRangeAsync(dto.FilmingLocations);

            return Result.Ok(movie.Id);
        }
        
        public async Task<Result> UpdateMovieAsync(Guid id, CreateMovieDto dto, Stream? stream, string? fileExtension)
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

        public async Task<Result> DeleteMovieAsync(Guid id)
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

        public async Task<Result<MovieWithReviewsDto>> GetMovieAsync(Guid movieId, Guid? userId, int reviewCount = 10)
        {
            var movie = await _movieRepository.GetMovieWithReviewsAsync(movieId, userId);

            if (movie == null)
            {
                return Result.Fail(new Error("Movie not exist").WithMetadata("Code", "MovieNotExist"));
            }

            return Result.Ok(movie);
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

        private string GetFilePath(Guid movieId)
        {
            var result = _fileService.GetFilePath(Path.Combine(MoviePath, $"{movieId}.jpg"));
            return result.IsSuccess ? result.Value : "/images/no-image.webp";
        }
    }
}