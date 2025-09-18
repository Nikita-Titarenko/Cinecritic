using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.MovieUsers;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMovieUserService _movieUserService;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IMovieUserService movieUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _movieUserService = movieUserService;
        }

        public async Task<Result<MovieUserStatusDto>> CreateMovieReviewAsync(UpsertMovieReviewDto dto)
        {
            var repo = _unitOfWork.MovieUsers;
            var movieUser = await repo.GetMovieUserWithReview(dto.MovieId, dto.UserId);
            if (movieUser == null)
            {
                await _movieUserService.DeleteFromWatchListAsync(dto.MovieId, dto.UserId);
                movieUser = _mapper.Map<MovieUser>(dto);
                movieUser.Review = new Review
                {
                    ReviewText = dto.ReviewText,
                    ReviewDateTime = DateTime.UtcNow
                };
                repo.Add(movieUser);
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

            var resultDto = _mapper.Map<MovieUserStatusDto>(movieUser);
            resultDto.IsWatched = true;
            return resultDto;
        }

        public async Task<Result<MovieUserStatusDto>> UpdateMovieReviewAsync(UpsertMovieReviewDto dto)
        {
            var repo = _unitOfWork.MovieUsers;
            var movieUser = await repo.GetMovieUserWithReview(dto.MovieId, dto.UserId);
            if (movieUser == null || movieUser.Review == null)
            {
                return Result.Fail(new Error("Movie review not exist").WithMetadata("Code", "MovieReviewNotExist"));
            }

            movieUser.Review.ReviewText = dto.ReviewText;
            movieUser.Review.ReviewDateTime = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            var resultDto = _mapper.Map<MovieUserStatusDto>(movieUser);
            resultDto.IsWatched = true;
            return resultDto;
        }

        public async Task<Result<IEnumerable<MovieReviewDto>>> GetMovieReviews(int movieId, int pageNumber, int pageSize)
        {
            return Result.Ok(await _unitOfWork.Reviews.GetMovieReviews(movieId, pageNumber, pageSize));
        }
    }
}
