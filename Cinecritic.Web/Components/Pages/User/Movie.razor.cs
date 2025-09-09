using Cinecritic.Application.Services.Movies;
using Cinecritic.Application.Services.Users;
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
        [Inject]
        private IMovieService MovieService { get; set; } = default!;

        //protected override async Task OnInitializedAsync()
        //{
        //    await MovieService.
        //    return base.OnInitializedAsync();
        //}
    }
}