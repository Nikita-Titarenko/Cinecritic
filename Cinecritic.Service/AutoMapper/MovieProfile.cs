using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieListItemDto>();
            CreateMap<CreateMovieDto, Movie>();
            CreateMap<Movie, MovieDto>()
                .ForPath(
                    dest => dest.MovieUserStatus.IsInWatchList,
                    opt => opt.MapFrom(src => src.WatchList.Any())
                )
                .ForPath(
                    dest => dest.MovieUserStatus.IsWatched,
                    opt => opt.MapFrom(src => src.MovieUsers.Any())
                )
                .ForPath(
                    dest => dest.MovieUserStatus.IsLiked,
                    opt => opt.MapFrom(src => src.MovieUsers.Any() && src.MovieUsers.First().IsLiked)
                )
                .ForPath(
                    dest => dest.MovieUserStatus.Rate,
                    opt => opt.MapFrom(src => src.MovieUsers.Select(mu => mu.Rate).FirstOrDefault())
                );
        }
    }
}
