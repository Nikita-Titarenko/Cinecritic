using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Web.ViewModels;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile() {
            CreateMap<CreateMovieViewModel, CreateMovieDto>().ForMember(dest => dest.MovieTypeId, opt => opt.MapFrom(src => src.SelectedMovieTypeId));
            CreateMap<MovieListItemDto, MovieListItemViewModel>();
        }
    }
}
