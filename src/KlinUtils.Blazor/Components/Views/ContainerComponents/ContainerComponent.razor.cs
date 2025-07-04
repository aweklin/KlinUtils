using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views.ContainerComponents;

public partial class ContainerComponent : ComponentBase
{
    [Parameter]
    public CurrentComponentState State { get; set; }

    [Parameter]
    public RenderFragment? Content { get; set; }

    [Parameter]
    public string? Error { get; set; }
}