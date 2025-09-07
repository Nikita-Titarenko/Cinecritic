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
        private const string MoviePath = "movie-posters";

        public MovieService(IMovieRepository movieRepository, IMapper mapper, IFileService fileService)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Result<int>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension)
        {
            var movieId = await _movieRepository.CreateMovieAsync(_mapper.Map<Movie>(dto));
            if (stream != null)
            {
                var path = Path.Combine(MoviePath, $"{movieId}{fileExtension}");
                await _fileService.SaveFile(path, stream);
            }

            return Result.Ok(movieId);
        }

        public async Task<Result<IEnumerable<MovieListItemDto>>> GetMoviesAsync(int pageSize, int pageCount)
        {
            var result = await _movieRepository.GetMoviesAsync(pageSize, pageCount);
            
            var movieList = _mapper.Map<IEnumerable<MovieListItemDto>>(result);

            foreach (var movie in movieList)
            {
                var getFilePathResult = _fileService.GetFilePath(Path.Combine(MoviePath, $"{movie.Id}.jpg"));
                if (getFilePathResult.IsSuccess)
                {
                    movie.ImagePath = getFilePathResult.Value;
                } else
                {
                    movie.ImagePath = "/images/no-image.webp";
                }
            }

            return Result.Ok(movieList);
        }
    }
}
