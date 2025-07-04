using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class FragmentComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderDefaultFragment_WhenNoParametersAreSet()
    {
        IRenderedComponent<FragmentComponent> cut = RenderComponent<FragmentComponent>();
        AngleSharp.Dom.IElement div = cut.Find("div");

        div.ClassName.Should().Be(string.Empty);
        div.GetAttribute("style").Should().Be(string.Empty);
        div.InnerHtml.Should().Be(string.Empty);
    }

    [Fact]
    public void ShouldRenderFragmentContent_WhenContentIsProvided()
    {
        IRenderedComponent<FragmentComponent> cut = RenderComponent<FragmentComponent>(p =>
            p.Add(c => c.Content, (builder) => builder.AddContent(0, "Hello from inside!")));

        cut.MarkupMatches("""
            <div class="" style="">
                Hello from inside!
            </div>
        """);
    }

    [Fact]
    public void ShouldApplyCssClassAndStyleAttributes()
    {
        IRenderedComponent<FragmentComponent> cut = RenderComponent<FragmentComponent>(p => p
            .Add(c => c.CssClass, "my-class px-4")
            .Add(c => c.Style, "background-color: pink; color: white;")
            .Add(c => c.Content, (builder) => builder.AddMarkupContent(0, "<span>🎉</span>")));

        AngleSharp.Dom.IElement div = cut.Find("div");

        div.ClassList.Should().Contain("my-class");
        div.ClassList.Should().Contain("px-4");
        div.GetAttribute("style").Should().Be("background-color: pink; color: white;");
        div.InnerHtml.Should().Contain("🎉");
    }
}