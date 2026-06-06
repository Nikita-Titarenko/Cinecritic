using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Files;
using Cinecritic.Application.Services.Reviews;
using Cinecritic.Domain.Models;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Cinecritic.Application.Services.Movies
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IReviewService _reviewService;
        private readonly IMemoryCache _cache;
        private readonly IMovieRepository _movieRep;
        private readonly IMovieUserRepository _movieUserRep;

        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        public const string MoviePath = "movie-posters";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, IReviewService reviewService, IMemoryCache cache, IMovieRepository movieRep, IMovieUserRepository movieUserRep)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _reviewService = reviewService;
            _cache = cache;
            _movieRep = movieRep;
            _movieUserRep = movieUserRep;
        }

        public async Task<Result<int>> CreateMovieAsync(CreateMovieDto dto, Stream? stream, string? fileExtension)
        {
            var movie = _mapper.Map<Movie>(dto);
            _movieRep.Add(movie);
            await _unitOfWork.CommitAsync();

            var movieId = movie.Id;

            if (stream != null)
            {
                var path = Path.Combine("movie-posters", $"{movieId}{fileExtension}");
                await _fileService.SaveFile(path, stream);
            }

            // ClearMovieCache();
            return Result.Ok(movieId);
        }

        public async Task<Result<int>> UpdateMovieAsync(int movieId, CreateMovieDto dto, Stream? stream, string? fileExtension)
        {
            var movie = await _movieRep.GetAsync(movieId);
            if (movie == null)
            {
                return Result.Fail(new Error("Movie not exist").WithMetadata("Code", "MovieNotExist"));
            }

            if (stream != null)
            {
                var filePath = GetFilePath(movieId);
                File.Delete(filePath);
                var path = GetFilePath(movieId);
                await _fileService.SaveFile(path, stream);
            }

            _movieRep.Update(_mapper.Map(dto, movie));
            await _unitOfWork.CommitAsync();

            ClearMovieCache();
            return Result.Ok(movieId);
        }

        public async Task<Result<int>> DeleteMovieAsync(int movieId)
        {
            var movie = await _movieRep.GetAsync(movieId);
            if (movie == null)
            {
                return Result.Fail(new Error("Movie not exist").WithMetadata("Code", "MovieNotExist"));
            }
            _movieRep.Delete(movie);
            var filePath = GetFilePath(movieId);
            File.Delete(filePath);
            await _unitOfWork.CommitAsync();

            ClearMovieCache();
            return Result.Ok(movieId);
        }

        public async Task<Result<GetMoviesResultDto>> GetMoviesAsync(int pageSize, int pageCount)
        {
            var cacheKey = $"movies_page_{pageCount}_{pageSize}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));

                var movies = await _movieRep.GetMoviesAsync(pageSize, pageCount);
                foreach (var movie in movies)
                {
                    movie.ImagePath = GetFilePath(movie.Id);
                }

                return Result.Ok(new GetMoviesResultDto
                {
                    Movies = movies,
                    TotalMovieNumber = await _movieRep.CountAsync()
                });
            }) ?? Result.Fail<GetMoviesResultDto>("Error loading movies from cache");
        }

        public async Task<Result<MovieDto>> GetMovieAsync(int movieId, int userId, int reviewCount = 10)
        {
            var cacheKey = $"movie_details_{movieId}_user_{userId}_rev_{reviewCount}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));

                var movie = await _movieUserRep.GetMovieAsync(movieId, userId);
                if (movie == null)
                {
                    return Result.Fail<MovieDto>(new Error("Movie not exist").WithMetadata("Code", "MovieNotExist"));
                }
                var result = await _reviewService.GetMovieReviews(movieId, 1, reviewCount);
                if (!result.IsSuccess)
                {
                    return Result.Fail<MovieDto>(new Error("Reviews not exist").WithMetadata("Code", "ReviewsNotExist"));
                }
                movie.Reviews = result.Value;
                movie.ImagePath = GetFilePath(movie.Id);
                return Result.Ok(movie);
            }) ?? Result.Fail<MovieDto>("Error loading movie details from cache");
        }

        public async Task<IEnumerable<TopMovieQueryResult>> GetTopMoviesByTypeAsync(
            int movieTypeId,
            decimal minRating,
            int pageNumber,
            int pageSize,
            string? userId)
        {
            var cacheKey = $"top_movies_type_{movieTypeId}_rate_{minRating}_page_{pageNumber}_size_{pageSize}_user_{userId ?? "anonymous"}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));

                return await _movieRep.GetTopMoviesByTypeAsync(
                    movieTypeId,
                    minRating,
                    pageNumber,
                    pageSize,
                    userId);
            }) ?? Enumerable.Empty<TopMovieQueryResult>();
        }

        private string GetFilePath(int movieId)
        {
            var result = _fileService.GetFilePath(Path.Combine(MoviePath, $"{movieId}.jpg"));
            return result.IsSuccess ? result.Value : "/images/no-image.webp";
        }

        public static void ClearMovieCache()
        {
            var currentToken = _resetCacheToken;
            _resetCacheToken = new CancellationTokenSource();
            currentToken.Cancel();
            currentToken.Dispose();
        }
    }
}