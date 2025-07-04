using KlinUtils.Blazor.Components.Models;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class AlertComponent : ComponentBase
{
    [Parameter]
    public AlertType Type { get; set; } = AlertType.None;

    [Parameter]
    public string Message { get; set; } = string.Empty;

    [Parameter]
    public IEnumerable<string> Data { get; set; } = [];

    public AlertComponent SetType(AlertType alertType)
    {
        Type = alertType;
        return this;
    }

    public AlertComponent SetMessage(string message)
    {
        Message = message;
        return this;
    }

    public AlertComponent SetData(IEnumerable<string> data)
    {
        Data = data;
        return this;
    }

    private string GetStyles(string? classes = null)
    {
        return Type switch
        {
            AlertType.Info => classes ?? "text-blue-700 bg-blue-200",
            AlertType.Warning => classes ?? "text-orange-700 bg-orange-200",
            AlertType.Error => classes ?? "text-red-700 bg-red-200",
            AlertType.Success => classes ?? "text-green-700 bg-green-300",
            AlertType.None => string.Empty,
            _ => throw new NotImplementedException(),
        };
    }
}