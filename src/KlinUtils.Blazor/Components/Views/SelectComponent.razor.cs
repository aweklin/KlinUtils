using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class SelectComponent<T> : ComponentBase
{
    private T _internalValue = default!;

    [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
    public T Value
#pragma warning restore BL0007 // Component parameters should be auto properties
    {
        get => _internalValue;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(value, default!) &&
                !EqualityComparer<T>.Default.Equals(_internalValue, value))
            {
                _internalValue = value;
                _ = ValueChanged.InvokeAsync(value);
            }
        }
    }

    [Parameter]
    public string CssClass { get; set; } = "w-full mt-1 px-4 py-2 border rounded focus:ring-green-500 focus:border-green-500";

    [Parameter]
    public string Placeholder { get; set; } = string.Empty;

    [Parameter]
    public IEnumerable<SelectItem> Items { get; set; } = [];

    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsRequired { get; set; }

    public bool IsEnabled => !IsDisabled;

    private static T DefaultValue =>
        typeof(T) == typeof(string)
            ? (T)(object)string.Empty
            : typeof(T) == typeof(int) || typeof(T) == typeof(double)
                ? (T)(object)0
                : default!;

    public async Task SetValue(T value)
    {
        Value = value;
        await InvokeAsync(() => ValueChanged.InvokeAsync(Value)).ConfigureAwait(false);
    }

    public Task OnValueChanged(ChangeEventArgs changeEventArgs)
    {
        string stringValue = changeEventArgs?.Value?.ToString() ?? string.Empty;
        Value = ConvertValue(stringValue);
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

    private static T ConvertValue(string value)
    {
        return typeof(T) == typeof(string)
            ? (T)(object)value!
            : typeof(T).IsEnum
            ? (T)Enum.Parse(typeof(T), value)
            : typeof(T) == typeof(int)
            ? (T)(object)int.Parse(value, System.Globalization.CultureInfo.InvariantCulture)
            : typeof(T) == typeof(long)
            ? (T)(object)long.Parse(value, System.Globalization.CultureInfo.InvariantCulture)
            : typeof(T) == typeof(double)
            ? (T)(object)double.Parse(value, System.Globalization.CultureInfo.InvariantCulture)
            : typeof(T) == typeof(decimal)
            ? (T)(object)decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture)
            : typeof(T) == typeof(bool)
            ? (T)(object)bool.Parse(value)
            : throw new InvalidOperationException($"Cannot convert '{value}' to type {typeof(T)}.");
    }
}