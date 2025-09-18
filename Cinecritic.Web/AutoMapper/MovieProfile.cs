using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Application.DTOs.MovieUsers;
using Cinecritic.Web.ViewModels;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile() {
            CreateMap<CreateMovieViewModel, CreateMovieDto>()
                .ForMember(dest => dest.MovieTypeId, opt => opt.MapFrom(src => src.SelectedMovieTypeId));
            CreateMap<MovieListItemDto, MovieListItemViewModel>();
            CreateMap<MovieDto, MovieViewModel>();
            CreateMap<MovieUserStatusDto, MovieUserStatusViewModel>()
                .ForMember(dest => dest.IsReviewCreated, opt => opt.MapFrom(src => src.ReviewText != null));
        }
    }
}
