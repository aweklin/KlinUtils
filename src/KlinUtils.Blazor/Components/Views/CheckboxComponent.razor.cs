using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class CheckboxComponent : ComponentBase
{
    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<bool> IsCheckedChanged { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsChecked { get; set; }

    public bool IsEnabled => !IsDisabled;

    public async Task SetChecked(bool isChecked)
    {
        IsChecked = isChecked;
        await InvokeAsync(() => IsCheckedChanged.InvokeAsync(IsChecked)).ConfigureAwait(false);
    }

    public Task ToggleCheckChanged()
    {
        IsChecked = !IsChecked;
        return IsCheckedChanged.InvokeAsync(IsChecked);
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