using System.Diagnostics;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views.ContainerComponents;

public partial class ContainerStatesComponent : ComponentBase
{
    [Parameter]
    public CurrentComponentState State { get; set; }

    [Parameter]
    public RenderFragment? LoadingFragment { get; set; }

    [Parameter]
    public RenderFragment? ContentFragment { get; set; }

    [Parameter]
    public RenderFragment? ErrorFragment { get; set; }

    private RenderFragment GetComponentStateFragment()
    {
        return State switch
        {
            CurrentComponentState.Loading => LoadingFragment!,
            CurrentComponentState.Content => ContentFragment!,
            CurrentComponentState.Error => ErrorFragment!,
            _ => throw new UnreachableException(),
        };
    }
}