using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web.Components.Pages.User
{
    public partial class Movie
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    }
}