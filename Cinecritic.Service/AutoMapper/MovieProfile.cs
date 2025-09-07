using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinecritic.Application.DTOs.Movies;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.AutoMapper
{
    public class MovieProfile : Profile
    {
        public MovieProfile() {
            CreateMap<Movie, MovieListItemDto>();
            CreateMap<CreateMovieDto, Movie>();
        }
    }
}
