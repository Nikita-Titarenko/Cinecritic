using Cinecritic.Web.JSInterop;

namespace Cinecritic.Web;

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
