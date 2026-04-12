using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cinecritic.Web.Components.Pages.Account
{
    public partial class Register
    {
        private string? _statusMessage;

        [Inject]
        private IUserService UserService { get; set; } = null!;

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private IMapper Mapper { get; set; } = null!;

        [Inject]
        private IJSRuntime JS { get; set; } = null!;

        private EditContext EditContext = null!;
        private ValidationMessageStore validationMessageStore = null!;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new InputModel();

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EditContext = new EditContext(Input);
            validationMessageStore = new ValidationMessageStore(EditContext);
        }

        public async Task RegisterUser()
        {
            var registerResult = await UserService.RegisterAsync(Mapper.Map<RegisterDto>(Input));

            if (!registerResult.IsSuccess)
            {
                if (registerResult.Errors.Any(e => e.Metadata.TryGetValue("Code", out var code) && code.ToString() == "EmailAlreadyExist"))
                {
                    validationMessageStore.Add(EditContext.Field(nameof(Input.Email)), "Email already exist");
                }
                else
                {
                    _statusMessage = "Error in service";
                }
                return;
            }

            Navigation.NavigateTo($"/api/auth/login-callback?userId={registerResult.Value.UserId}", forceLoad: true);
        }

        public sealed class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string DisplayName { get; set; } = "";

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = "";

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";
        }
    }
}