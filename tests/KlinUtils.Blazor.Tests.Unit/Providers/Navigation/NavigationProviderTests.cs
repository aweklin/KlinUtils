using KlinUtils.Blazor.Providers.Navigation;

using Microsoft.JSInterop;

using NSubstitute;

namespace KlinUtils.Blazor.Tests.Unit.Providers.Navigation;

public class NavigationProviderTests
{
    private readonly FakeNavigationManager _navigationManager = new();
#pragma warning disable CA1859 // Use concrete types when possible for improved performance

    private readonly IJSRuntime _jsRuntime = Substitute.For<IJSRuntime>();

    private readonly INavigationProvider _navigationProvider;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

    public NavigationProviderTests()
    {
        _navigationProvider = new NavigationProvider(_navigationManager, _jsRuntime);
    }

    [Fact]
    public void Constructor_WithNullNavigationManager_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new NavigationProvider(null!, _jsRuntime));
    }

    [Fact]
    public void Constructor_WithNullJsRuntime_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new NavigationProvider(_navigationManager, null!));
    }

    [Fact]
    public void BaseUrl_ReturnsBaseUri()
    {
        Assert.Equal("https://example.com/", _navigationProvider.BaseUrl);
    }

    [Theory]
    [InlineData("route", "https://example.com/route")]
    [InlineData("/route", "https://example.com/route")]
    [InlineData("deep/nest", "https://example.com/deep/nest")]
    public void ToAbsoluteUrl_ValidInput_ReturnsExpectedAbsolute(string relative, string expected)
    {
        string result = _navigationProvider.ToAbsoluteUrl(relative);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NavigateTo_InvalidRoute_Throws(string? route)
    {
        Assert.Throws<ArgumentException>(() => _navigationProvider.NavigateTo(route ?? string.Empty));
    }

    [Fact]
    public async Task BackAsync_InvokesHistoryBack()
    {
        await _navigationProvider.BackAsync();

        var call = Assert.Single(_jsRuntime.ReceivedCalls());

        Assert.Equal(
            nameof(IJSRuntime.InvokeAsync),
            call.GetMethodInfo().Name);

        Assert.Equal(
            "history.back",
            call.GetArguments()[0]);
    }

    [Fact]
    public async Task ForwardAsync_InvokesHistoryForward()
    {
        await _navigationProvider.ForwardAsync();

        var call = Assert.Single(_jsRuntime.ReceivedCalls());

        Assert.Equal(
            "history.forward",
            call.GetArguments()[0]);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(-5)]
    public async Task GotoAsync_InvokesHistoryGo(int delta)
    {
        await _navigationProvider.GotoAsync(delta);

        var call = Assert.Single(_jsRuntime.ReceivedCalls());

        Assert.Equal(
            "history.go",
            call.GetArguments()[0]);

        object[] args = (object[])call.GetArguments()[2]!;

        Assert.Single(args);
        Assert.Equal(delta, args[0]);
    }

#pragma warning disable SA1129 // Do not use default value type constructor
    [Fact]
    public async Task BackAsync_PassesCancellationToken()
    {
        CancellationToken cancellationToken = new();

        await _navigationProvider.BackAsync(cancellationToken);

        var call = Assert.Single(_jsRuntime.ReceivedCalls());

        Assert.Equal(
            cancellationToken,
            call.GetArguments()[1]);
    }

    [Fact]
    public async Task GotoAsync_PassesCancellationToken()
    {
        CancellationToken cancellationToken = new();

        await _navigationProvider.GotoAsync(-1, cancellationToken);

        var call = Assert.Single(_jsRuntime.ReceivedCalls());

        Assert.Equal("history.go", call.GetArguments()[0]);

        Assert.Equal(
            cancellationToken,
            call.GetArguments()[1]);

        object[] args = (object[])call.GetArguments()[2]!;

        Assert.Single(args);
        Assert.Equal(-1, args[0]);
    }
#pragma warning restore SA1129 // Do not use default value type constructor
}