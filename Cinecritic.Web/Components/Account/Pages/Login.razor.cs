using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace Cinecritic.Web.Components.Account.Pages
{
    public partial class Login
    {
        private string? statusMessage;
        [Inject]
        private IUserService UserService { get; set; } = default!;
        [Inject]
        private IMapper Mapper { get; set; } = default!;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (HttpMethods.IsGet(HttpContext.Request.Method))
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }
        }

        public async Task LoginUser()
        {
            var loginResult = await UserService.LoginAsync(Mapper.Map<LoginDto>(Input));
            if (!loginResult.IsSuccess)
            {
                object? code;
                if (loginResult.Errors.Any(e => e.Metadata.TryGetValue("Code", out code) && code.ToString() == "LoginFailed"))
                {
                    statusMessage = "Error: Email or password incorrect";
                }
                else
                {
                    statusMessage = "Error in service";
                }

                return;
            }

            RedirectManager.RedirectTo(ReturnUrl);
        }

        public sealed class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}