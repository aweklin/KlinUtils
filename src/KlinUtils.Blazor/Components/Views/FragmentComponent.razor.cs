using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class FragmentComponent : ComponentBase
{
    /// <summary>
    /// Gets or sets the content of the fragment.
    /// </summary>
    [Parameter]
    public RenderFragment? Content { get; set; }

    /// <summary>
    /// Gets or sets the CSS class for the fragment.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the style for the fragment.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;
}