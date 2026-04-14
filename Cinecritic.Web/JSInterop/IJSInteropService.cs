
using Microsoft.JSInterop;

namespace Cinecritic.Web.JSInterop;

internal interface IJSInteropService
{
    Task AddScrollHandler<T>(DotNetObjectReference<T> dotNetObjectReference) where T : class;
    Task BlurActiveElement();
    Task ShowAlertAsync();
}