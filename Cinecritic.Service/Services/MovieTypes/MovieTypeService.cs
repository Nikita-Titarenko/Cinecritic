using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes
{
    public class MovieTypeService : IMovieTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MovieTypeDto>>> GetMovieTypes()
        {
            var watchListEntities = await _unitOfWork.Repository<MovieType>().GetAllAsync();
            var dto = _mapper.Map<IEnumerable<MovieTypeDto>>(watchListEntities);
            return Result.Ok(dto);
        }
    }
}
