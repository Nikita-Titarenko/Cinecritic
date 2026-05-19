using AutoMapper;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Application.DTOs.Reviews;
using Cinecritic.Web.ViewModels.Movies;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieUserProfile : Profile
    {
        public MovieUserProfile()
        {
            CreateMap<MovieViewModel, RateMovieDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.MovieUserStatus.ApplicationUserId));
            CreateMap<MovieViewModel, UpsertMovieReviewDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.MovieUserStatus.ApplicationUserId))
                .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.MovieUserStatus.ReviewText));
        }
    }
}
