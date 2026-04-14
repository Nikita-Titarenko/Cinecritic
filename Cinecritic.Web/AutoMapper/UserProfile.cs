using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Web.Components.Pages.Account;
using Register = Cinecritic.Web.Components.Pages.Account.Register;

namespace Cinecritic.Web.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Register.InputModel, RegisterDto>();
        CreateMap<Login.InputModel, LoginDto>();
    }
}
