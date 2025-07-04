using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class NumberTextFieldComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderWithDefaultValue()
    {
        IRenderedComponent<NumberTextFieldComponent<int>> cut = RenderComponent<NumberTextFieldComponent<int>>();

        AngleSharp.Dom.IElement input = cut.Find("input");
        input.GetAttribute("value").Should().Be("0"); // default int value
    }

    [Fact]
    public void ShouldRenderWithCustomValueAndAttributes()
    {
        IRenderedComponent<NumberTextFieldComponent<decimal>> cut = RenderComponent<NumberTextFieldComponent<decimal>>(p => p
            .Add(c => c.Value, 12.5m)
            .Add(c => c.Placeholder, "Enter number")
            .Add(c => c.CssClass, "rounded-md border")
            .Add(c => c.IsRequired, true)
            .Add(c => c.IsDisabled, true));

        AngleSharp.Dom.IElement input = cut.Find("input");
        input.GetAttribute("value").Should().Be("12.5");
        input.GetAttribute("placeholder").Should().Be("Enter number");
        input.ClassList.Should().Contain("rounded-md");
        input.HasAttribute("required").Should().BeTrue();
        input.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public async Task SetValue_ShouldUpdateValueAndTriggerEventCallback()
    {
        int? updated = null;

        IRenderedComponent<NumberTextFieldComponent<int>> cut = RenderComponent<NumberTextFieldComponent<int>>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<int>(this, val => updated = val)));

        await cut.Instance.SetValue(99);

        cut.Instance.Value.Should().Be(99);
        updated.Should().Be(99);
    }

    [Fact]
    public async Task OnValueChanged_ShouldParseAndFireCallback()
    {
        int? updated = null;

        IRenderedComponent<NumberTextFieldComponent<int>> cut = RenderComponent<NumberTextFieldComponent<int>>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<int>(this, val => updated = val)));

        AngleSharp.Dom.IElement input = cut.Find("input");

        await input.InputAsync(new ChangeEventArgs { Value = "123" });

        cut.Instance.Value.Should().Be(123);
        updated.Should().Be(123);
    }

    [Fact]
    public async Task OnValueChanged_ShouldDefaultToZeroWhenInputIsNull()
    {
        int? updated = null;

        IRenderedComponent<NumberTextFieldComponent<int>> cut = RenderComponent<NumberTextFieldComponent<int>>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<int>(this, val => updated = val)));

        AngleSharp.Dom.IElement input = cut.Find("input");

        await input.InputAsync(new ChangeEventArgs { Value = null });

        cut.Instance.Value.Should().Be(0);
        updated.Should().Be(0);
    }

    [Fact]
    public void Enable_ShouldSetIsEnabled()
    {
        IRenderedComponent<NumberTextFieldComponent<double>> cut = RenderComponent<NumberTextFieldComponent<double>>(p => p.Add(c => c.IsDisabled, true));
        cut.Instance.Enable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("input").HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void Disable_ShouldSetIsDisabled()
    {
        IRenderedComponent<NumberTextFieldComponent<double>> cut = RenderComponent<NumberTextFieldComponent<double>>(p => p.Add(c => c.IsDisabled, false));
        cut.Instance.Disable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }
}