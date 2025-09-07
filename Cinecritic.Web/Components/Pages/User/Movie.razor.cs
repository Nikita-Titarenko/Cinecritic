using Microsoft.AspNetCore.Components;

namespace Cinecritic.Web.Components.Pages.User
{
    public partial class Movie
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;
    }
}