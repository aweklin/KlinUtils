using System.Globalization;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Components.Views;

public sealed partial class DateOnlyComponent : ComponentBase
{
  public string Type { get; } = "date";

  [Parameter]
  public DateOnly Value { get; set; }

  [Parameter]
  public string CssClass { get; set; } = "w-full mt-1 px-4 py-2 border rounded focus:ring-purple-500 focus:border-purple-500";

  [Parameter]
  public string Placeholder { get; set; } = string.Empty;

  [Parameter]
  public EventCallback<DateOnly> ValueChanged { get; set; }

  [Parameter]
  public bool IsDisabled { get; set; }

  [Parameter]
  public bool IsRequired { get; set; }

  public bool IsEnabled => !IsDisabled;

  public string ValueToDisplay
  {
    get
    {
      return Value == default ? string.Empty : Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
  }

  public async Task SetValue(DateOnly value)
  {
    Value = value;
    await InvokeAsync(() => ValueChanged.InvokeAsync(Value)).ConfigureAwait(false);
  }

  public Task OnValueChanged(ChangeEventArgs changeEventArgs)
  {
    string input = changeEventArgs?.Value?.ToString() ?? string.Empty;
    if (!DateOnly.TryParse(input, out DateOnly date))
    {
      return Task.CompletedTask;
    }

    return SetValue(date);
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