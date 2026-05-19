using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Cinecritic.Application.DTOs.Account;
using Cinecritic.Web.Components.Account.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Profile
{
    public partial class Index
    {
        private string statusMessage = string.Empty;
        private string displayName = string.Empty;
        private bool isCenturion;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;
        
        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var profileResult = await UserService.GetUserProfileAsync(userId);

            if (profileResult.IsSuccess)
            {
                displayName = profileResult.Value.Name;
                isCenturion = profileResult.Value.IsCenturion;
            }
            else
            {
                displayName = HttpContext.User.FindFirst("DisplayName")?.Value ?? string.Empty;
            }

            if (string.IsNullOrEmpty(Input.DisplayName))
            {
                Input.DisplayName = displayName;
            }
        }

        private async Task OnValidSubmitAsync()
        {
            if (Input.DisplayName != displayName)
            {
                var changeDisplayNameResult = await UserService.ChangeDisplayNameAsync(new ChangeDisplayNameDto
                {
                    DisplayName = Input.DisplayName,
                    UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                });
                if (!changeDisplayNameResult.IsSuccess)
                {
                    statusMessage = "Error in service";
                    return;
                }
                displayName = Input.DisplayName;
                statusMessage = "Your profile has been updated";
            }
        }

        private sealed class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string DisplayName { get; set; } = string.Empty;
        }
    }
}