using Microsoft.JSInterop;

namespace Cinecritic.Web.JSInterop
{
    public class JSInteropService : IJSInteropService
    {
        private readonly IJSRuntime _jSRuntime;

        public JSInteropService(IJSRuntime jSRuntime) {
            _jSRuntime = jSRuntime;
        }

        public async Task ShowAlertAsync()
        {
            await _jSRuntime.InvokeVoidAsync("showAlert");
        }

        public async Task AddScrollHandler<T>(DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            await _jSRuntime.InvokeVoidAsync("addScrollHandler", dotNetObjectReference);
        }
    }
}
