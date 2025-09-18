using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinecritic.Application.DTOs.MovieTypes;
using Cinecritic.Domain.Models;

namespace Cinecritic.Application.AutoMapper
{
    public class MovieTypeProfile : Profile
    {
        public MovieTypeProfile() {
            CreateMap<MovieType, MovieTypeDto>();
        }
    }
}
