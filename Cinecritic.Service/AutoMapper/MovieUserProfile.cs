using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.AutoMapper
{
    public class MovieUserProfile : Profile
    {
        public MovieUserProfile()
        {
            CreateMap<MovieUser, MovieUserStatusDto>()
                .ForMember(dest => dest.IsWatched, opt => opt.MapFrom(src => true));
            CreateMap<DateTime, DateOnly>()
                .ConvertUsing(src => DateOnly.FromDateTime(src));
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(dest => dest.MovieType, opt => opt.MapFrom(src => new MovieType
                {
                    Id = src.MovieTypeId,
                    Name = src.MovieTypeName,
                }));
        }
    }
}
