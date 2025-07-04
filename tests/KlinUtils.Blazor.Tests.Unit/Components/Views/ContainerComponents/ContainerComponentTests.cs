using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views.ContainerComponents;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views.ContainerComponents;

public class ContainerComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderLoadingFragment_WhenStateIsLoading()
    {
        IRenderedComponent<ContainerComponent> cut = RenderComponent<ContainerComponent>(p =>
            p.Add(c => c.State, CurrentComponentState.Loading)
                .Add(c => c.Content, builder => builder.AddContent(0, "Real content"))
                .Add(c => c.Error, "Oops!"));

        cut.Markup.Should().Contain("Loading...");
        cut.Markup.Should().NotContain("Real content");
        cut.Markup.Should().NotContain("Oops!");
    }

    [Fact]
    public void ShouldRenderContentFragment_WhenStateIsContent()
    {
        IRenderedComponent<ContainerComponent> cut = RenderComponent<ContainerComponent>(p =>
            p.Add(c => c.State, CurrentComponentState.Content)
                .Add(c => c.Content, builder => builder.AddContent(0, "✅ Loaded"))
                .Add(c => c.Error, "Nope"));

        cut.Markup.Should().Contain("✅ Loaded");
        cut.Markup.Should().NotContain("Loading...");
        cut.Markup.Should().NotContain("Nope");
    }

    [Fact]
    public void ShouldRenderErrorFragment_WhenStateIsError()
    {
        IRenderedComponent<ContainerComponent> cut = RenderComponent<ContainerComponent>(p =>
            p.Add(c => c.State, CurrentComponentState.Error)
                .Add(c => c.Content, builder => builder.AddContent(0, "👍"))
                .Add(c => c.Error, "Something failed"));

        cut.Markup.Should().Contain("Something failed");
        cut.Markup.Should().NotContain("👍");
        cut.Markup.Should().NotContain("Loading...");
    }

    [Fact]
    public void ShouldRenderCorrectFragment_BasedOnInternalState()
    {
        IRenderedComponent<ContainerStatesComponent> cut = RenderComponent<ContainerStatesComponent>(p =>
            p.Add(c => c.State, CurrentComponentState.Error)
             .Add(c => c.LoadingFragment, b => b.AddContent(0, "Please wait"))
             .Add(c => c.ContentFragment, b => b.AddContent(0, "Everything's fine"))
             .Add(c => c.ErrorFragment, b => b.AddContent(0, "Something went wrong")));

        cut.Markup.Should().Contain("Something went wrong");
    }
}