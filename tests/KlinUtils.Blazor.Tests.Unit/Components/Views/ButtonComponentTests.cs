using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class ButtonComponentTests : TestContext
{
    [Fact]
    public void ShouldDefaultButtonWithLabel()
    {
        IRenderedComponent<ButtonComponent> cut = RenderComponent<ButtonComponent>();

        cut.MarkupMatches($"""
            <button type="button"  class="text-black border border-gray-300 rounded-md px-6 py-3 mb-3">
                <div>Button</div>
            </button>
        """);
    }

    [Fact]
    public void ShouldRenderButtonWithCustomLabel()
    {
        IRenderedComponent<ButtonComponent> cut =
            RenderComponent<ButtonComponent>(p => p.Add(prop => prop.Label, "Click Me"));

        cut.Find("div").TextContent.Should().Be("Click Me");
    }

    [Fact]
    public void ShouldRenderContentInsteadOfLabel()
    {
        IRenderedComponent<ButtonComponent> cut = RenderComponent<ButtonComponent>(p =>
            p.Add(prop => prop.Content, (builder) => builder.AddContent(0, "Custom Content")));

        cut.Markup.Contains("Custom Content", StringComparison.OrdinalIgnoreCase).Should().BeTrue();
        cut.Markup.Should().NotContain("Button");
    }

    [Fact]
    public void ShouldTriggerClickCallback()
    {
        bool clicked = false;
        IRenderedComponent<ButtonComponent> cut =
            RenderComponent<ButtonComponent>(p => p.Add(prop => prop.OnClick, () => clicked = true));

        cut.Find("button").Click();

        Assert.True(clicked);
    }

    [Fact]
    public void ShouldRespectIsDisabledState()
    {
        IRenderedComponent<ButtonComponent> cut =
            RenderComponent<ButtonComponent>(p => p.Add(prop => prop.IsDisabled, true));

        AngleSharp.Dom.IElement button = cut.Find("button");
        button.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Enable_ShouldMakeButtonEnabled()
    {
        IRenderedComponent<ButtonComponent> cut =
            RenderComponent<ButtonComponent>(p => p.Add(prop => prop.IsDisabled, true));

        cut.Instance.Enable();
        cut.Render(); // Force re-render after manual change

        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("button").HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void Disable_ShouldMakeButtonDisabled()
    {
        IRenderedComponent<ButtonComponent> cut =
            RenderComponent<ButtonComponent>(p => p.Add(prop => prop.IsDisabled, false));

        cut.Instance.Disable();
        cut.Render(); // Force re-render

        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("button").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void SetLabel_ShouldUpdateLabel()
    {
        IRenderedComponent<ButtonComponent> cut = RenderComponent<ButtonComponent>();
        cut.Instance.SetLabel("Updated");
        cut.Render();

        cut.Find("div").TextContent.Should().Be("Updated");
    }

    [Fact]
    public void ShouldRenderImage_WhenUseImageIsTrueAndImageUrlIsProvided()
    {
        Uri imageUri = new Uri("https://example.com/icon.png");

        IRenderedComponent<ButtonComponent> cut = RenderComponent<ButtonComponent>(parameters => parameters
            .Add(p => p.UseImage, true)
            .Add(p => p.ImageUrl, imageUri)
            .Add(p => p.ImageAlt, "Sample icon")
            .Add(p => p.ImageCssClass, "w-5 h-5"));

        AngleSharp.Dom.IElement img = cut.Find("img");
        img.GetAttribute("src").Should().Be(imageUri.ToString());
        img.GetAttribute("alt").Should().Be("Sample icon");
        img.ClassList.Should().Contain("w-5");
        img.ClassList.Should().Contain("h-5");
    }

    [Fact]
    public void ShouldNotRenderImage_WhenUseImageIsFalse()
    {
        IRenderedComponent<ButtonComponent> cut = RenderComponent<ButtonComponent>(parameters => parameters
            .Add(p => p.UseImage, false)
            .Add(p => p.ImageUrl, new Uri("https://example.com/icon.png")));

        cut.FindAll("img").Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotRenderImage_WhenImageUrlIsNull()
    {
        IRenderedComponent<ButtonComponent> cut = RenderComponent<ButtonComponent>(parameters => parameters
            .Add(p => p.UseImage, true)
            .Add(p => p.ImageUrl, null));

        cut.FindAll("img").Should().BeEmpty();
    }
}