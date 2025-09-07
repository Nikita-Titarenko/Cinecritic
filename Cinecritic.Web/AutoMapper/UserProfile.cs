using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Web.Components.Account.Pages;
namespace Cinecritic.Web.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<Register.InputModel, RegisterDto>();
        }
    }
}
