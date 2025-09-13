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
            CreateMap<RateMovieDto, MovieUser>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate));
            CreateMap<WatchList, MovieUserStatusDto>()
                .ForMember(dest => dest.IsWatched, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsLiked, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => (int?)null));
            CreateMap<MovieUser, MovieUserStatusDto>()
                .ForMember(dest => dest.IsWatched, opt => opt.MapFrom(src => true));
        }
    }
}
