
using Microsoft.JSInterop;

namespace Cinecritic.Web.JSInterop
{
    interface IJSInteropService
    {
        Task AddScrollHandler<T>(DotNetObjectReference<T> dotNetObjectReference) where T : class;
        Task ShowAlertAsync();
    }
}