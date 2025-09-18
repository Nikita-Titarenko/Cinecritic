using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Reviews;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IReviewService _reviewService;
        private const string MoviePath = "movie-posters";

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IReviewService reviewService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _reviewService = reviewService;
        }

        public async Task<Result<int>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension)
        {
            var movie = _mapper.Map<Movie>(dto);
            _unitOfWork.Repository<Movie>().Add(movie);
            await _unitOfWork.CommitAsync();

            var movieId = movie.Id;

            if (stream != null)
            {
                var path = Path.Combine("movie-posters", $"{movieId}{fileExtension}");
                await _fileService.SaveFile(path, stream);
            }

            return Result.Ok(movieId);
        }

        public async Task<Result<IEnumerable<MovieListItemDto>>> GetMoviesAsync(int pageSize, int pageCount)
        {
            var movies = await _unitOfWork.Movies.GetMoviesAsync(pageSize, pageCount);
            foreach (var movie in movies)
            {
                movie.ImagePath = GetFilePath(movie.Id);
            }
            return Result.Ok(movies);
        }

        public async Task<Result<MovieDto>> GetMovieAsync(int movieId, string userId, int reviewCount = 10)
        {
            var movie = await _unitOfWork.Movies.GetMovieAsync(movieId, userId);
            if (movie == null)
            {
                return Result.Fail(new Error("Movie not exist").WithMetadata("Code", "MovieNotExist"));
            }
            var result = await _reviewService.GetMovieReviews(movieId, 1, reviewCount);
            if (!result.IsSuccess)
            {
                return Result.Fail(new Error("Reviews not exist").WithMetadata("Code", "ReviewsNotExist"));
            }
            movie.Reviews = result.Value;
            movie.ImagePath = GetFilePath(movie.Id);
            return Result.Ok(movie);
        }

        private string GetFilePath(int movieId)
        {
            var result = _fileService.GetFilePath(Path.Combine(MoviePath, $"{movieId}.jpg"));
            return result.IsSuccess ? result.Value : "/images/no-image.webp";
        }
    }
}
