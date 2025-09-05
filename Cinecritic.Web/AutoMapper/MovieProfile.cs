using AutoMapper;
using Cinecritic.Application.DTOs;
using Cinecritic.Web.ViewModels;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile() {
            CreateMap<CreateMovieViewModel, CreateMovieDto>();
            CreateMap<MovieListItemDto, MovieListItemViewModel>();
        }
    }
}
