using System.Globalization;

using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class DateOnlyComponentTests : TestContext
{
  [Fact]
  public void ShouldRenderWithDefaultAttributes()
  {
    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>();

    AngleSharp.Dom.IElement input = cut.Find("input");

    input.GetAttribute("type").Should().Be("date");
    input.GetAttribute("value").Should().Be(string.Empty);
    input.ClassList.Should().Contain("w-full");
    input.HasAttribute("placeholder").Should().BeTrue();
    input.GetAttribute("autocomplete").Should().Be("off");
    input.HasAttribute("required").Should().BeFalse();
    input.HasAttribute("disabled").Should().BeFalse();
  }

  [Fact]
  public void ShouldRenderWithCustomProperties()
  {
    DateOnly value = new(2026, 11, 19);

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p => p
            .Add(c => c.Value, value)
            .Add(c => c.Placeholder, "Select date")
            .Add(c => c.CssClass, "bg-white border-gray-300")
            .Add(c => c.IsRequired, true)
            .Add(c => c.IsDisabled, true));

    AngleSharp.Dom.IElement input = cut.Find("input");

    input.GetAttribute("value")
        .Should()
        .Be("2026-11-19");

    input.GetAttribute("placeholder")
        .Should()
        .Be("Select date");

    input.ClassList.Should().Contain("bg-white");

    input.HasAttribute("required").Should().BeTrue();
    input.HasAttribute("disabled").Should().BeTrue();
  }

  [Fact]
  public async Task SetValue_ShouldUpdateValueAndFireCallback()
  {
    DateOnly? updated = null;

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p =>
            p.Add(
                c => c.ValueChanged,
                EventCallback.Factory.Create<DateOnly>(
                    this,
                    value => updated = value)));

    DateOnly expected = new(2026, 11, 19);

    await cut.Instance.SetValue(expected);

    cut.Instance.Value.Should().Be(expected);
    updated.Should().Be(expected);
  }

  [Fact]
  public async Task OnValueChanged_ShouldCaptureInputEvent()
  {
    DateOnly? updated = null;

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p =>
            p.Add(
                c => c.ValueChanged,
                EventCallback.Factory.Create<DateOnly>(
                    this,
                    value => updated = value)));

    AngleSharp.Dom.IElement input =
        cut.Find("input");

    DateOnly expected = new(2026, 11, 19);

    await input.InputAsync(
        new ChangeEventArgs
        {
          Value = expected.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
        });

    cut.Instance.Value.Should().Be(expected);
    updated.Should().Be(expected);
  }

  [Fact]
  public async Task OnValueChanged_WithInvalidDate_ShouldNotUpdateValue()
  {
    DateOnly original = new(2026, 01, 01);

    DateOnly? updated = null;

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p => p
            .Add(c => c.Value, original)
            .Add(
                c => c.ValueChanged,
                EventCallback.Factory.Create<DateOnly>(
                    this,
                    value => updated = value)));

    AngleSharp.Dom.IElement input =
        cut.Find("input");

    await input.InputAsync(
        new ChangeEventArgs
        {
          Value = "Not-A-Date",
        });

    cut.Instance.Value.Should().Be(original);

    updated.Should().BeNull();
  }

  [Fact]
  public async Task OnValueChanged_WithNullValue_ShouldNotUpdateValue()
  {
    DateOnly original = new(2026, 01, 01);

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p =>
            p.Add(c => c.Value, original));

    await cut.Instance.OnValueChanged(
        new ChangeEventArgs
        {
          Value = null,
        });

    cut.Instance.Value.Should().Be(original);
  }

  [Fact]
  public void Enable_Disable_ShouldToggleFieldState()
  {
    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(
            p => p.Add(c => c.IsDisabled, true));

    cut.Instance.Enable();
    cut.Render();

    cut.Instance.IsEnabled.Should().BeTrue();
    cut.Find("input")
        .HasAttribute("disabled")
        .Should()
        .BeFalse();

    cut.Instance.Disable();
    cut.Render();

    cut.Instance.IsEnabled.Should().BeFalse();
    cut.Find("input")
        .HasAttribute("disabled")
        .Should()
        .BeTrue();
  }

  [Fact]
  public void Value_ShouldRenderInIsoFormat()
  {
    DateOnly value = new(2026, 12, 31);

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p =>
            p.Add(c => c.Value, value));

    cut.Find("input")
        .GetAttribute("value")
        .Should()
        .Be("2026-12-31");
  }

  [Fact]
  public void DefaultDateValue_ShouldRenderEmptyString()
  {
    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p =>
            p.Add(c => c.Value, default));

    cut.Find("input")
        .GetAttribute("value")
        .Should()
        .Be(string.Empty);
  }

  [Theory]
  [InlineData("2024-02-29")]
  [InlineData("2025-01-01")]
  [InlineData("2030-12-31")]
  public async Task OnValueChanged_ShouldAcceptValidDates(
      string dateText)
  {
    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>();

    await cut.Instance.OnValueChanged(
        new ChangeEventArgs
        {
          Value = dateText,
        });

    cut.Instance.Value.Should().Be(
        DateOnly.Parse(dateText, CultureInfo.InvariantCulture));
  }

  [Theory]
  [InlineData("")]
  [InlineData("abc")]
  [InlineData("99/99/9999")]
  [InlineData("invalid")]
  public async Task OnValueChanged_ShouldIgnoreInvalidDates(
      string input)
  {
    DateOnly original = new(2026, 01, 01);

    IRenderedComponent<DateOnlyComponent> cut =
        RenderComponent<DateOnlyComponent>(p =>
            p.Add(c => c.Value, original));

    await cut.Instance.OnValueChanged(
        new ChangeEventArgs
        {
          Value = input,
        });

    cut.Instance.Value.Should().Be(original);
  }
}