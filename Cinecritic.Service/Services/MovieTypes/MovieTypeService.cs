using AutoMapper;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes
{
    public class MovieTypeService : IMovieTypeService
    {
        private readonly IRepository<MovieType> _movieTypeRep;
        private readonly IMapper _mapper;

        public MovieTypeService(IRepository<MovieType> movieTypeRep, IMapper mapper)
        {
            _movieTypeRep = movieTypeRep;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<MovieType>>> GetMovieTypes()
        {
            var movieTypes = await _movieTypeRep.GetAllAsync();
            return Result.Ok(movieTypes);
        }
    }
}
