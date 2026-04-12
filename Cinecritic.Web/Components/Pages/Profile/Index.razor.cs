using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Cinecritic.Application.DTOs.Account;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Pages.Profile
{
    public partial class Index : BasePage
    {
        private string? _statusMessage;
        private string _displayName = string.Empty;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var userId = await GetUserIdAsync();
            var getNameResult = await UserService.GetNameAsync(userId!.Value);
            _displayName = getNameResult.IsSuccess ? getNameResult.Value : string.Empty;
            if (string.IsNullOrEmpty(Input.DisplayName))
            {
                Input.DisplayName = _displayName;
            }
        }

        private async Task OnValidSubmitAsync()
        {
            if (Input.DisplayName != _displayName)
            {
                var userId = await GetUserIdAsync();

                var changeDisplayNameResult = await UserService.ChangeDisplayNameAsync(new ChangeDisplayNameDto
                {
                    DisplayName = Input.DisplayName,
                    UserId = userId!.Value
                });
                if (!changeDisplayNameResult.IsSuccess)
                {
                    _statusMessage = "Error in service";
                    return;
                }
                _statusMessage = "Your profile has been updated";
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