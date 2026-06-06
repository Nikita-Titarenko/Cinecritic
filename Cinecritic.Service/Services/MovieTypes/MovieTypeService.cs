using AutoMapper;
using Cinecritic.Application.DTOs.MovieTypes;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes
{
    public class MovieTypeService : IMovieTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MovieType> _movieTypeRepository;
        private readonly IMapper _mapper;

        public MovieTypeService(
            IUnitOfWork unitOfWork,
            IRepository<MovieType> movieTypeRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _movieTypeRepository = movieTypeRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MovieTypeDto>>> GetMovieTypes()
        {
            var watchListEntities = await _movieTypeRepository.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<MovieTypeDto>>(watchListEntities);
            return Result.Ok(dto);
        }
    }
}