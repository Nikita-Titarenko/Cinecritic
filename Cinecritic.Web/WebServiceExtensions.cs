using Cinecritic.Web.Components.Pages.Account;
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
            services.AddScoped<IJSInteropService, JSInteropService>();
            return services;
        }
    }
}
