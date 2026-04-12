using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Services.Users;
using Microsoft.AspNetCore.Components;

namespace Cinecritic.Web.Components.Pages.Account
{
    public partial class Login
    {
        private string? _statusMessage;
        [Inject]
        private IUserService UserService { get; set; } = null!;
        [Inject]
        private IMapper Mapper { get; set; } = null!;
        
        [Inject]
        private NavigationManager Navigation { get; set; } = null!;

        private InputModel Input { get; set; } = new();
        
        public async Task LoginUser()
        {
            var loginResult = await UserService.LoginAsync(Mapper.Map<LoginDto>(Input));
            if (!loginResult.IsSuccess)
            {
                if (loginResult.Errors.Any(e => e.Metadata.TryGetValue("Code", out var code) && code.ToString() == "LoginFailed"))
                {
                    _statusMessage = "Error: Email or password incorrect";
                }
                else
                {
                    _statusMessage = "Error in service";
                }
        
                return;
            }
        
            Navigation.NavigateTo($"/api/auth/login-callback?userId={loginResult.Value.UserId}", forceLoad: true);
        }
        
        public sealed class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";
        
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";
        }
    }
}