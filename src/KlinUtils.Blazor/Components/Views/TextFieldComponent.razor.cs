using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class TextFieldComponent : ComponentBase
{
    [Parameter]
    public string Type { get; set; } = "text";

    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = "w-full mt-1 px-4 py-2 border rounded focus:ring-green-500 focus:border-green-500";

    [Parameter]
    public string Placeholder { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsRequired { get; set; }

    public bool IsEnabled => !IsDisabled;

    public async Task SetValue(string value)
    {
        Value = value;
        await InvokeAsync(() => ValueChanged.InvokeAsync(Value)).ConfigureAwait(false);
    }

    public Task OnValueChanged(ChangeEventArgs changeEventArgs)
    {
        Value = changeEventArgs?.Value?.ToString() ?? string.Empty;
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