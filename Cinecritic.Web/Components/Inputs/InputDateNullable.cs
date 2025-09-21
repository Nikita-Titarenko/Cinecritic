using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Cinecritic.Web.Components.Inputs
{
    public class InputDateNullable : InputBase<DateOnly?>
    {
        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateOnly? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = null;
                validationErrorMessage = string.Empty;
                return true;
            }

            DateOnly parse;
            if (!DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parse))
            {
                result = null;
                validationErrorMessage = "Date is not valid";
                return false;
            }
            result = parse;
            validationErrorMessage = string.Empty;
            return true;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", "date");
            builder.AddAttribute(2, "class", CssClass);
            builder.AddAttribute(3, "value", CurrentValue.HasValue
                ? CurrentValue.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                : string.Empty);
            builder.AddAttribute(4, "onchange", EventCallback.Factory.CreateBinder<string?>(
                this, __value => CurrentValueAsString = __value, CurrentValueAsString));
            builder.CloseElement();
        }
    }
}
