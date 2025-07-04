using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public partial class NumberTextFieldComponent<T> : ComponentBase
{
    [Parameter]
    public T Value { get; set; } = default!;

    [Parameter]
    public string CssClass { get; set; } = "w-full mt-1 px-4 py-2 border rounded focus:ring-green-500 focus:border-green-500";

    [Parameter]
    public string Placeholder { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsRequired { get; set; }

    public bool IsEnabled => !IsDisabled;

    protected string CurrentValueAsString
    {
        get => (string)BindConverter.FormatValue(Value)!;
        set
        {
            if (BindConverter.TryConvertTo<T>(value, CultureInfo.InvariantCulture, out T? parsedValue))
            {
                Value = parsedValue;
                ValueChanged.InvokeAsync(Value);
            }
        }
    }

    public async Task SetValue(T value)
    {
        Value = value;
        await InvokeAsync(() => ValueChanged.InvokeAsync(Value)).ConfigureAwait(false);
    }

    public Task OnValueChanged([NotNull] ChangeEventArgs changeEventArgs)
    {
        CurrentValueAsString = changeEventArgs.Value?.ToString() ?? "0";

        return ValueChanged.InvokeAsync(Value);
    }

    public void Disable()
    {
        IsDisabled = true;
        InvokeAsync(StateHasChanged);
    }

    public void Enable()
    {
        IsDisabled = false;
        InvokeAsync(StateHasChanged);
    }
}