using AutoMapper;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Domain.Models;
using Cinecritic.Web.ViewModels;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieUserProfile : Profile
    {
        public MovieUserProfile() {
            CreateMap<MovieViewModel, RateMovieDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.MovieUserStatus.UserId));
        }
    }
}
