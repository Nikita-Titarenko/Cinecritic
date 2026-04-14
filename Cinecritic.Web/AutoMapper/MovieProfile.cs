using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Web.ViewModels.Movies;

namespace Cinecritic.Web.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<CreateMovieViewModel, CreateMovieDto>()
                .ForMember(dest => dest.MovieTypeId, opt => opt.MapFrom(src => src.SelectedMovieTypeId));
            CreateMap<GetMoviesResultDto, MovieListViewModel>();
            CreateMap<MovieWithReviewsDto, CreateMovieViewModel>();
        }
    }
}
