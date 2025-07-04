using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class ModalDialogComponent : ComponentBase
{
    [Parameter]
    public RenderFragment? ModalContent { get; set; } = null!;

    [Parameter]
    public bool IsVisible { get; set; }

    public void Show()
    {
        IsVisible = true;
    }

    public void Hide()
    {
        IsVisible = false;
    }
}