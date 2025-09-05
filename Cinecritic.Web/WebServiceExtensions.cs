using Cinecritic.Application.Repositories;
using Cinecritic.Application.Services.Movies;
using Cinecritic.Infrastructure.Repositories;
using Cinecritic.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cinecritic.Web
{
    public static class WebServiceExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<IdentityUserAccessor>();
            services.AddCascadingAuthenticationState();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            return services;
        }
    }
}
