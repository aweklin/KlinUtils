namespace KlinUtils.Blazor.Components.Views.ContainerComponents;

public enum CurrentComponentState
{
    /// <summary>
    /// Indicates that the component is in its loading state, yet to be rendered on the screen.
    /// </summary>
    Loading = 0,

    /// <summary>
    /// Indicates that the component is rendered on the screen.
    /// </summary>
    Content,

    /// <summary>
    /// Indicates that the component encountered an error while rendering.
    /// </summary>
    Error,
}