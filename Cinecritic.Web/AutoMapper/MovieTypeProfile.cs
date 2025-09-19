using AutoMapper;
using Cinecritic.Application.DTOs.MovieTypes;
using Cinecritic.Web.ViewModels.MovieTypes;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieTypeProfile : Profile
    {
        public MovieTypeProfile() {
            CreateMap<MovieTypeDto, MovieTypeViewModel>();
        }
    }
}
