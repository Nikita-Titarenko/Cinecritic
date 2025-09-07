using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.Repositories;
using FluentResults;

namespace Cinecritic.Application.Services.MovieType
{
    public class MovieTypeService : IMovieTypeService
    {
        private readonly IMovieTypeRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieTypeService(IMovieTypeRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<MovieTypeDto>>> GetMovieTypes()
        {
            var movieTypes = await _movieRepository.GetMovieTypes();
            return Result.Ok(_mapper.Map<IEnumerable<MovieTypeDto>>(movieTypes));
        }
    }
}
