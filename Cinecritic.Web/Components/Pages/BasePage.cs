using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MongoDB.Bson;

namespace Cinecritic.Web.Components.Pages;

public class BasePage : ComponentBase
{
    [Inject]
    private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    protected async Task<ObjectId?> GetUserIdAsync()
    {
        var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userId = auth.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId == null ? null : ObjectId.Parse(userId);
    }
}