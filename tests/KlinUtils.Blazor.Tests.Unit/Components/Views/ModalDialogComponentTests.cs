using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class ModalDialogComponentTests : TestContext
{
    [Fact]
    public void ShouldNotRenderModal_WhenIsVisibleIsFalse()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, false)
                .Add(c => c.ModalContent, b => b.AddContent(0, "Hello Modal")));

        // Assert that content isn't rendered
        cut.Markup.Should().Be(string.Empty);
    }

    [Fact]
    public void ShouldRenderModalContent_WhenIsVisibleIsTrue()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, true)
                .Add(c => c.ModalContent, b => b.AddContent(0, "Welcome to the modal!")));

        cut.MarkupMatches("""
            <div class="relative z-10" id="otp-modal" aria-labelledby="modal-title" role="dialog" aria-modal="true">
                <div class="fixed inset-0 bg-gray-500/75 transition-opacity app-modal" aria-hidden="true"></div>
                <div class="fixed inset-0 z-10 w-screen overflow-y-auto">
                    <div class="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                    <div style="font-size: 14px;" class="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-xl p-4">Welcome to the modal!</div>
                    </div>
                </div>
            </div>
        """);
    }

    [Fact]
    public void Show_ShouldMakeModalVisible()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, false)
                .Add(c => c.ModalContent, b => b.AddContent(0, "Dynamic Content")));

        cut.Instance.Show();
        cut.Render(); // force re-render after programmatic update

        cut.Markup.Should().Contain("Dynamic Content");
    }

    [Fact]
    public void Hide_ShouldMakeModalDisappear()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, true)
                .Add(c => c.ModalContent, b => b.AddContent(0, "Closing soon...")));

        cut.Instance.Hide();
        cut.Render();

        cut.Markup.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotRender_WhenIsVisibleIsFalse()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, false)
             .Add(c => c.ModalContent, b => b.AddContent(0, "Nothing to see here.")));

        cut.Markup.Should().BeEmpty(); // Entire block skipped
    }

    [Fact]
    public void ShouldRenderFullModalMarkup_WhenIsVisibleIsTrue()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, true)
             .Add(c => c.ModalContent, b => b.AddContent(0, "🚀 Modal loaded")));

        cut.Find("#otp-modal").Should().NotBeNull();
        cut.Find("#otp-modal").GetAttribute("role").Should().Be("dialog");
        cut.Find(".app-modal").ClassList.Should().Contain("transition-opacity");

        cut.Markup.Should().Contain("🚀 Modal loaded");
        cut.Markup.Should().Contain("class=\"relative transform overflow-hidden");
    }

    [Fact]
    public void Show_ShouldToggleVisibilityToTrue()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, false)
             .Add(c => c.ModalContent, b => b.AddContent(0, "Animated pop")));

        cut.Instance.Show();
        cut.Render();

        cut.Markup.Should().Contain("Animated pop");
    }

    [Fact]
    public void Hide_ShouldToggleVisibilityToFalse()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, true)
             .Add(c => c.ModalContent, b => b.AddContent(0, "Closing...")));

        cut.Instance.Hide();
        cut.Render();

        cut.Markup.Should().BeEmpty();
    }

    [Fact]
    public void ShouldRespectAccessibilityAttributes()
    {
        IRenderedComponent<ModalDialogComponent> cut = RenderComponent<ModalDialogComponent>(p =>
            p.Add(c => c.IsVisible, true)
             .Add(c => c.ModalContent, b => b.AddContent(0, "<strong>Secure Dialog</strong>")));

        AngleSharp.Dom.IElement modal = cut.Find("div[role='dialog']");
        modal.HasAttribute("aria-modal").Should().BeTrue();
        modal.GetAttribute("id").Should().Be("otp-modal");
        modal.GetAttribute("aria-labelledby").Should().Be("modal-title");
    }
}