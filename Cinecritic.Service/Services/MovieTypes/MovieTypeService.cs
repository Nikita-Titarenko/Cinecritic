using AutoMapper;
using Cinecritic.Application.Repositories;
using Cinecritic.Domain.Models;
using FluentResults;

namespace Cinecritic.Application.Services.MovieTypes;

public class MovieTypeService(IRepository<MovieType> movieTypeRep, IMapper mapper) : IMovieTypeService
{
    private readonly IRepository<MovieType> _movieTypeRep = movieTypeRep;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<IEnumerable<MovieType>>> GetMovieTypes()
    {
        var movieTypes = await _movieTypeRep.GetAllAsync();
        return Result.Ok(movieTypes);
    }
}
