using AutoMapper;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.DTOs.Reviews;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Domain.Models;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Cinecritic.Application.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieUserRepository _movieUserRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IMovieUserService _movieUserService;
        private readonly IMemoryCache _cache;

        private static CancellationTokenSource _resetReviewCacheToken = new CancellationTokenSource();
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public ReviewService(
            IUnitOfWork unitOfWork, 
            IMovieUserRepository movieUserRepository,
            IReviewRepository reviewRepository,
            IMapper mapper, 
            IMovieUserService movieUserService, 
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _movieUserRepository = movieUserRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _movieUserService = movieUserService;
            _cache = cache;
        }

        public async Task<Result<MovieUserStatusDto>> CreateMovieReviewAsync(UpsertMovieReviewDto dto)
        {
            var movieUser = await _movieUserRepository.GetMovieUserWithReviewAsync(dto.MovieId, dto.ApplicationUserId);
            if (movieUser == null)
            {
                await _movieUserService.DeleteFromWatchListAsync(dto.MovieId, dto.ApplicationUserId);
                movieUser = _mapper.Map<MovieUser>(dto);
                movieUser.Review = new Review
                {
                    ReviewText = dto.ReviewText,
                    ReviewDateTime = DateTime.UtcNow
                };
                _movieUserRepository.Add(movieUser);
            }
            else
            {
                if (movieUser.Review != null)
                {
                    return Result.Fail(new Error("Movie review already exist").WithMetadata("Code", "MovieReviewAlreadyExist"));
                }
                else
                {
                    movieUser.Review = new Review
                    {
                        ReviewText = dto.ReviewText,
                        ReviewDateTime = DateTime.UtcNow
                    };
                }
            }
            await _unitOfWork.CommitAsync();

            ClearCache();

            var resultDto = _mapper.Map<MovieUserStatusDto>(movieUser);
            resultDto.IsWatched = true;
            return resultDto;
        }

        public async Task<Result<MovieUserStatusDto>> UpdateMovieReviewAsync(UpsertMovieReviewDto dto)
        {
            var movieUser = await _movieUserRepository.GetMovieUserWithReviewAsync(dto.MovieId, dto.ApplicationUserId);
            if (movieUser == null || movieUser.Review == null)
            {
                return Result.Fail(new Error("Movie review not exist").WithMetadata("Code", "MovieReviewNotExist"));
            }

            movieUser.Review.ReviewText = dto.ReviewText;
            movieUser.Review.ReviewDateTime = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            ClearCache();

            var resultDto = _mapper.Map<MovieUserStatusDto>(movieUser);
            resultDto.IsWatched = true;
            return resultDto;
        }

        public async Task<Result<IEnumerable<MovieReviewDto>>> GetMovieReviews(int movieId, int pageNumber, int pageSize)
        {
            var cacheKey = $"movie_reviews_id_{movieId}_page_{pageNumber}_size_{pageSize}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.AddExpirationToken(new CancellationChangeToken(_resetReviewCacheToken.Token));

                return Result.Ok(await _reviewRepository.GetMovieReviews(movieId, pageNumber, pageSize));
            }) ?? Result.Fail<IEnumerable<MovieReviewDto>>("Error loading reviews from cache");
        }

        private void ClearCache()
        {
            var currentToken = _resetReviewCacheToken;
            _resetReviewCacheToken = new CancellationTokenSource();
            currentToken.Cancel();
            currentToken.Dispose();
            
            MovieService.ClearMovieCache();
        }
    }
}