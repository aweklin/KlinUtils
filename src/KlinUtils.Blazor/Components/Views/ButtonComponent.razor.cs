using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class ButtonComponent : ComponentBase
{
    [Parameter]
    public string Type { get; set; } = "button";

    [Parameter]
    public string? Label { get; set; } = "Button";

    [Parameter]
    public RenderFragment? Content { get; set; }

    [Parameter]
    public bool UseImage { get; set; }

    [Parameter]
    public Uri? ImageUrl { get; set; }

    [Parameter]
    public string? ImageAlt { get; set; }

    [Parameter]
    public string ImageCssClass { get; set; } = "w-5 h-5";

    [Parameter]
    public string CssClass { get; set; } = "text-black border border-gray-300 rounded-md px-6 py-3 mb-3";

    [Parameter]
    public bool IsDisabled { get; set; }

    public bool IsEnabled => !IsDisabled;

    [Parameter]
    public Action OnClick { get; set; } = () => { };

    public void SetLabel(string value)
    {
        Label = value;
    }

    public void Click()
    {
        OnClick?.Invoke();
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