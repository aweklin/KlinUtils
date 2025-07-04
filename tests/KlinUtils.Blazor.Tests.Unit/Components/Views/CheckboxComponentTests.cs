using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class CheckboxComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderWithLabel_WhenProvided()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.Label, "Accept Terms"));

        AngleSharp.Dom.IElement label = cut.Find("span");
        label.TextContent.Should().Be("Accept Terms");
    }

    [Fact]
    public void ShouldNotRenderLabel_WhenNullOrWhitespace()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.Label, "   "));

        cut.FindAll("span").Should().BeEmpty();
    }

    [Fact]
    public void ShouldRenderUnchecked_WhenIsCheckedIsFalse()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.IsChecked, false));

        AngleSharp.Dom.IElement checkboxDiv = cut.Find("div.w-5");
        checkboxDiv.ClassList.Should().Contain("bg-white");
        cut.FindAll("svg").Should().BeEmpty();
    }

    [Fact]
    public void ShouldRenderChecked_WhenIsCheckedIsTrue()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.IsChecked, true));

        AngleSharp.Dom.IElement checkboxDiv = cut.Find("div.w-5");
        checkboxDiv.ClassList.Should().Contain("bg-green-600");
        cut.Find("svg").Should().NotBeNull();
    }

    [Fact]
    public void ShouldRenderDisabled_WhenIsDisabledIsTrue()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.IsDisabled, true));

        AngleSharp.Dom.IElement input = cut.Find("input[type='checkbox']");
        input.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public async Task SetChecked_ShouldUpdateValueAndTriggerCallback()
    {
        bool? callbackValue = null;
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.IsCheckedChanged, EventCallback.Factory.Create<bool>(this, b => callbackValue = b)));

        await cut.Instance.SetChecked(true);
        cut.Instance.IsChecked.Should().BeTrue();
        callbackValue.Should().BeTrue();
    }

    [Fact]
    public async Task ToggleCheckChanged_ShouldToggleStateAndFireCallback()
    {
        bool? callbackValue = null;
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p
            .Add(c => c.IsChecked, false)
            .Add(c => c.IsCheckedChanged, EventCallback.Factory.Create<bool>(this, b => callbackValue = b)));

        await cut.Instance.ToggleCheckChanged();

        cut.Instance.IsChecked.Should().BeTrue();
        callbackValue.Should().BeTrue();
    }

    [Fact]
    public void Enable_ShouldSetIsDisabledFalse()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.IsDisabled, true));

        cut.Instance.Enable();
        cut.Render(); // force re-render

        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("input").HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void Disable_ShouldSetIsDisabledTrue()
    {
        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p.Add(c => c.IsDisabled, false));

        cut.Instance.Disable();
        cut.Render(); // force re-render

        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void ClickingWrapper_ShouldToggleCheckAndTriggerCallback()
    {
        bool? callbackResult = null;

        IRenderedComponent<CheckboxComponent> cut =
            RenderComponent<CheckboxComponent>(p => p
            .Add(c => c.IsCheckedChanged, EventCallback.Factory.Create<bool>(this, b => callbackResult = b)));

        cut.Find("div.inline-flex").Click();

        cut.Instance.IsChecked.Should().BeTrue();
        callbackResult.Should().BeTrue();
    }
}