using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class SelectComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderEmptySelect_WhenNoItemsAndNoPlaceholder()
    {
        IRenderedComponent<SelectComponent<int>> cut = RenderComponent<SelectComponent<int>>();

        AngleSharp.Dom.IElement select = cut.Find("select");
        select.Children.Length.Should().Be(0);
    }

    [Fact]
    public void ShouldRenderPlaceholderOption_WhenPlaceholderIsSet()
    {
        IRenderedComponent<SelectComponent<int>> cut = RenderComponent<SelectComponent<int>>(p =>
            p.Add(c => c.Placeholder, "Select a number"));

        AngleSharp.Dom.IElement option = cut.Find("select").Children[0];
        option.TextContent.Should().Be("Select a number");
        option.GetAttribute("value").Should().Be("0"); // default value for int
    }

    [Fact]
    public void ShouldRenderItems_AsOptions()
    {
        SelectItem[] items =
        [
            new SelectItem("1", "One"),
            new SelectItem("2", "Two"),
            new SelectItem("3", "Three")
        ];

        IRenderedComponent<SelectComponent<int>> cut = RenderComponent<SelectComponent<int>>(p =>
            p.Add(c => c.Items, items)
                 .Add(c => c.Placeholder, "Select"));

        IRefreshableElementCollection<AngleSharp.Dom.IElement> options = cut.FindAll("option");
        options.Should().HaveCount(4); // placeholder + 3 items
        options[1].TextContent.Should().Be("One");
        options[1].GetAttribute("value").Should().Be("1");
    }

    [Fact]
    public async Task OnValueChanged_ShouldConvertToIntAndInvokeCallback()
    {
        int? selected = null;

        IRenderedComponent<SelectComponent<int>> cut = RenderComponent<SelectComponent<int>>(p =>
            p.Add(c => c.Items, [new SelectItem("5", "Five")])
             .Add(c => c.ValueChanged, EventCallback.Factory.Create<int>(this, val => selected = val)));

        AngleSharp.Dom.IElement select = cut.Find("select");
        await select.ChangeAsync(new ChangeEventArgs() { Value = "5" });

        cut.Instance.Value.Should().Be(5);
        selected.Should().Be(5);
    }

    [Fact]
    public async Task OnValueChanged_ShouldConvertToEnumAndInvokeCallback()
    {
        TestColor? selected = null;

        IRenderedComponent<SelectComponent<TestColor>> cut = RenderComponent<SelectComponent<TestColor>>(p =>
            p.Add(
                c => c.Items,
                [
                    new SelectItem("Red", "Red"),
                    new SelectItem("Green", "Green")
                ])
                .Add(c => c.ValueChanged, EventCallback.Factory.Create<TestColor>(this, val => selected = val)));

        AngleSharp.Dom.IElement select = cut.Find("select");
        await select.ChangeAsync(new ChangeEventArgs() { Value = "Green" });

        cut.Instance.Value.Should().Be(TestColor.Green);
        selected.Should().Be(TestColor.Green);
    }

    [Fact]
    public async Task SetValue_ShouldSetAndTriggerCallback()
    {
        int? selected = null;
        IRenderedComponent<SelectComponent<int>> cut = RenderComponent<SelectComponent<int>>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<int>(this, val => selected = val)));

        await cut.Instance.SetValue(42);
        cut.Render();

        cut.Instance.Value.Should().Be(42);
        selected.Should().Be(42);
    }

    [Fact]
    public void EnableDisable_ShouldToggleIsDisabledFlag()
    {
        IRenderedComponent<SelectComponent<string>> cut = RenderComponent<SelectComponent<string>>(p =>
            p.Add(c => c.IsDisabled, false));

        cut.Instance.Disable();
        cut.Render();
        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("select").HasAttribute("disabled").Should().BeTrue();

        cut.Instance.Enable();
        cut.Render();
        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("select").HasAttribute("disabled").Should().BeFalse();
    }
}

#pragma warning disable SA1201 // Elements should appear in the correct order

public enum TestColor
#pragma warning restore SA1201 // Elements should appear in the correct order
{
#pragma warning disable SA1602 // Enumeration items should be documented
    Red,
    Green,
    Blue,
#pragma warning restore SA1602 // Enumeration items should be documented
}