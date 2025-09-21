using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using AutoMapper;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Application.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Cinecritic.Web.Components.Account.Pages
{
    public partial class Register
    {
        private string? statusMessage;

        [Inject]
        private IUserService UserService { get; set; } = default!;

        [Inject]
        private IMapper Mapper { get; set; } = default!;


        private EditContext EditContext = default!;
        private ValidationMessageStore validationMessageStore = default!;
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
                object? code;
                if (registerResult.Errors.Any(e => e.Metadata.TryGetValue("Code", out code) && code.ToString() == "EmailAlreadyExist"))
                {
                    validationMessageStore.Add(EditContext.Field(nameof(Input.Email)), "Email already exist");
                }
                else
                {
                    statusMessage = "Error in service";
                }

                return;
            }

            var callbackUrl = $"{NavigationManager.BaseUri}Account/ConfirmEmail?UserId={registerResult.Value.UserId}&Code={registerResult.Value.Code}";

            await EmailService.SendConfirmationLinkAsync(Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

            RedirectManager.RedirectTo(
                "Account/RegisterConfirmation",
                new() { ["email"] = Input.Email, ["returnUrl"] = ReturnUrl });
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