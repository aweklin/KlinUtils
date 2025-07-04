namespace KlinUtils.Blazor.Components.Models;

public enum AlertType
{
    /// <summary>
    /// Indicates that the alert is in its initial state.
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that the displayed message is information.
    /// </summary>
    Info,

    /// <summary>
    /// Indicates that the displayed message is a warning.
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates that the displayed message is an error.
    /// </summary>
    Error,

    /// <summary>
    /// Indicates that the displayed message is success.
    /// </summary>
    Success,
}