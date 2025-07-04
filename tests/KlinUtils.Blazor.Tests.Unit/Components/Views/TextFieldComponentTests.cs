using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class TextFieldComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderWithDefaultAttributes()
    {
        IRenderedComponent<TextFieldComponent> cut =
            RenderComponent<TextFieldComponent>();

        AngleSharp.Dom.IElement input = cut.Find("input");

        input.GetAttribute("type").Should().Be("text");
        input.GetAttribute("value").Should().Be(string.Empty);
        input.ClassList.Should().Contain("w-full");
        input.HasAttribute("placeholder").Should().BeTrue();
        input.HasAttribute("required").Should().BeFalse();
        input.HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void ShouldRenderWithCustomProperties()
    {
        IRenderedComponent<TextFieldComponent> cut =
            RenderComponent<TextFieldComponent>(p => p
            .Add(c => c.Type, "email")
            .Add(c => c.Value, "user@example.com")
            .Add(c => c.Placeholder, "Enter email")
            .Add(c => c.CssClass, "bg-white border-gray-300")
            .Add(c => c.IsRequired, true)
            .Add(c => c.IsDisabled, true));

        AngleSharp.Dom.IElement input = cut.Find("input");

        input.GetAttribute("type").Should().Be("email");
        input.GetAttribute("value").Should().Be("user@example.com");
        input.GetAttribute("placeholder").Should().Be("Enter email");
        input.ClassList.Should().Contain("bg-white");
        input.HasAttribute("required").Should().BeTrue();
        input.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public async Task SetValue_ShouldUpdateValueAndFireCallback()
    {
        string? updated = null;

        IRenderedComponent<TextFieldComponent> cut =
            RenderComponent<TextFieldComponent>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<string>(this, v => updated = v)));

        await cut.Instance.SetValue("new text");

        cut.Instance.Value.Should().Be("new text");
        updated.Should().Be("new text");
    }

    [Fact]
    public async Task OnValueChanged_ShouldCaptureInputEvent()
    {
        string? updated = null;

        IRenderedComponent<TextFieldComponent> cut =
            RenderComponent<TextFieldComponent>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<string>(this, v => updated = v)));

        AngleSharp.Dom.IElement input = cut.Find("input");

        await input.InputAsync(new ChangeEventArgs { Value = "typed value" });

        cut.Instance.Value.Should().Be("typed value");
        updated.Should().Be("typed value");
    }

    [Fact]
    public void Enable_Disable_ShouldToggleFieldState()
    {
        IRenderedComponent<TextFieldComponent> cut =
            RenderComponent<TextFieldComponent>(p => p.Add(c => c.IsDisabled, true));

        cut.Instance.Enable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("input").HasAttribute("disabled").Should().BeFalse();

        cut.Instance.Disable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }
}