using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class TextAreaComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderWithDefaultAttributes()
    {
        IRenderedComponent<TextAreaComponent> cut =
            RenderComponent<TextAreaComponent>();

        AngleSharp.Dom.IElement textarea = cut.Find("textarea");

        textarea.TextContent.Should().Be(string.Empty);
        textarea.ClassList.Should().Contain("w-full");
        textarea.HasAttribute("placeholder").Should().BeTrue();
        textarea.HasAttribute("required").Should().BeFalse();
        textarea.HasAttribute("disabled").Should().BeFalse();
        textarea.HasAttribute("rows").Should().BeTrue();
    }

    [Fact]
    public void ShouldRenderWithCustomProperties()
    {
        IRenderedComponent<TextAreaComponent> cut =
            RenderComponent<TextAreaComponent>(p => p
            .Add(c => c.Value, "user@example.com")
            .Add(c => c.Placeholder, "Enter email")
            .Add(c => c.CssClass, "bg-white border-gray-300")
            .Add(c => c.IsRequired, true)
            .Add(c => c.IsDisabled, true));

        AngleSharp.Dom.IElement textarea = cut.Find("textarea");

        textarea.TextContent.Should().Be("user@example.com");
        textarea.GetAttribute("placeholder").Should().Be("Enter email");
        textarea.ClassList.Should().Contain("bg-white");
        textarea.HasAttribute("required").Should().BeTrue();
        textarea.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public async Task SetValue_ShouldUpdateValueAndFireCallback()
    {
        string? updated = null;

        IRenderedComponent<TextAreaComponent> cut =
            RenderComponent<TextAreaComponent>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<string>(this, v => updated = v)));

        await cut.Instance.SetValue("new text");

        cut.Instance.Value.Should().Be("new text");
        updated.Should().Be("new text");
    }

    [Fact]
    public async Task OnValueChanged_ShouldCaptureInputEvent()
    {
        string? updated = null;

        IRenderedComponent<TextAreaComponent> cut =
            RenderComponent<TextAreaComponent>(p =>
            p.Add(c => c.ValueChanged, EventCallback.Factory.Create<string>(this, v => updated = v)));

        AngleSharp.Dom.IElement textArea = cut.Find("textarea");

        await textArea.InputAsync(new ChangeEventArgs { Value = "typed value" });

        cut.Instance.Value.Should().Be("typed value");
        updated.Should().Be("typed value");
    }

    [Fact]
    public void Enable_Disable_ShouldToggleFieldState()
    {
        IRenderedComponent<TextAreaComponent> cut =
            RenderComponent<TextAreaComponent>(p => p.Add(c => c.IsDisabled, true));

        cut.Instance.Enable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("textarea").HasAttribute("disabled").Should().BeFalse();

        cut.Instance.Disable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("textarea").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Enable_Disable_ShouldSetRows()
    {
        IRenderedComponent<TextAreaComponent> cut =
            RenderComponent<TextAreaComponent>(p => p.Add(c => c.Rows, 5));

        cut.Render();

        cut.Find("textarea").HasAttribute("rows").Should().BeTrue();
        cut.Instance.Rows.Should().Be(5);
    }
}