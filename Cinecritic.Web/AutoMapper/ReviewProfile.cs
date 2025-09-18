using System.Collections.Generic;
using AutoMapper;
using Cinecritic.Application.DTOs.Reviews;
using Cinecritic.Web.ViewModels;

namespace Cinecritic.Web.AutoMapper
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile() {
            CreateMap<MovieReviewDto, MovieReviewViewModel>();
        }
    }
}
