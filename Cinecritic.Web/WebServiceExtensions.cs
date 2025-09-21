using Cinecritic.Web.Components.Account;
using Cinecritic.Web.JSInterop;
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
            services.AddScoped<IJSInteropService, JSInteropService>();
            return services;
        }
    }
}
