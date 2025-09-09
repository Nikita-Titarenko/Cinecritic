using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Cinecritic.Application.DTOs.Account;
using Microsoft.AspNetCore.Components;

namespace Cinecritic.Web.Components.Profile
{
    public partial class Index
    {
        private string displayName = string.Empty;

        private string userId = string.Empty;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            displayName = auth.User.FindFirst("DisplayName")!.Value;
            if (string.IsNullOrEmpty(Input.DisplayName))
            {
                Input.DisplayName = displayName;
            }
            userId = auth.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        private async Task OnValidSubmitAsync()
        {
            if (Input.DisplayName != displayName)
            {
                var changeDisplayNameResult = await UserService.ChangeDisplayNameAsync(new ChangeDisplayNameDto
                {
                    DisplayName = Input.DisplayName,
                    UserId = userId
                });
                if (!changeDisplayNameResult.IsSuccess)
                {
                    RedirectManager.RedirectToCurrentPageWithStatus("Error: Failed to set phone number.", HttpContext);
                }
            }

            RedirectManager.RedirectToCurrentPageWithStatus("Your profile has been updated", HttpContext);
        }

        private sealed class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string DisplayName { get; set; } = string.Empty;
        }
    }
}