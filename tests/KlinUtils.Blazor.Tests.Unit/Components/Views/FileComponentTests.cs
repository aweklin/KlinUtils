using Bunit;

using FluentAssertions;

using KlinUtils.Blazor.Components.Views;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using NSubstitute;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class FileComponentTests : TestContext
{
    [Fact]
    public void ShouldRenderInputFile_WithDefaultClass()
    {
        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>();

        AngleSharp.Dom.IElement input = cut.Find("input[type='file']");
        input.ClassList.Should().Contain("w-full");
        input.HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void ShouldRenderDisabled_WhenIsDisabledIsTrue()
    {
        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>(p => p.Add(c => c.IsDisabled, true));

        AngleSharp.Dom.IElement input = cut.Find("input[type='file']");
        input.HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public void Enable_ShouldAllowFileInput()
    {
        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>(p => p.Add(c => c.IsDisabled, true));
        cut.Instance.Enable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeTrue();
        cut.Find("input").HasAttribute("disabled").Should().BeFalse();
    }

    [Fact]
    public void Disable_ShouldDisableFileInput()
    {
        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>(p => p.Add(c => c.IsDisabled, false));
        cut.Instance.Disable();
        cut.Render();

        cut.Instance.IsEnabled.Should().BeFalse();
        cut.Find("input").HasAttribute("disabled").Should().BeTrue();
    }

    [Fact]
    public async Task SetSelectedFile_ShouldTriggerEventCallback()
    {
        IBrowserFile file = Substitute.For<IBrowserFile>();
        IBrowserFile? selected = null;

        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>(p =>
            p.Add(c => c.SelectedFileChanged, EventCallback.Factory.Create<IBrowserFile>(this, f => selected = f)));

        await cut.Instance.SetSelectedFile(file);

        cut.Instance.SelectedFile.Should().Be(file);
        selected.Should().Be(file);
    }

    [Fact]
    public async Task OnSelectedFileChanged_ShouldUpdateSelectedFileAndTriggerCallback()
    {
        IBrowserFile file = Substitute.For<IBrowserFile>();
        IBrowserFile? selected = null;

        InputFileChangeEventArgs changeArgs = new([file]);

        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>(p =>
            p.Add(c => c.SelectedFileChanged, EventCallback.Factory.Create<IBrowserFile>(this, f => selected = f)));

        await cut.Instance.OnSelectedFileChanged(changeArgs);

        cut.Instance.SelectedFile.Should().Be(file);
        selected.Should().Be(file);
    }

    [Fact]
    public async Task OnSelectedFileChanged_ShouldThrow_IfArgsIsNull()
    {
        IRenderedComponent<FileComponent> cut = RenderComponent<FileComponent>();
        await Assert.ThrowsAsync<ArgumentNullException>(() => cut.Instance.OnSelectedFileChanged(null!));
    }
}