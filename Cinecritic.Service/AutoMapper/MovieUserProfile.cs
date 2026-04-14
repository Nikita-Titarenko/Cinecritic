using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.AutoMapper;

public class MovieUserProfile : Profile
{
    public MovieUserProfile()
    {
        CreateMap<CreateMovieDto, Movie>()
            .ForMember(dest => dest.MovieType, opt => opt.MapFrom(src => new MovieType
            {
                Id = src.MovieTypeId,
                Name = src.MovieTypeName,
            }));
    }
}
