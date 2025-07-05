using KlinUtils.Blazor.Providers.Navigation;

namespace KlinUtils.Blazor.Tests.Unit.Providers.Navigation;

public class NavigationProviderTests
{
    private readonly FakeNavigationManager _navigationManager = new();
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
    private readonly INavigationProvider _navigationProvider;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

    public NavigationProviderTests()
    {
        _navigationProvider = new NavigationProvider(_navigationManager);
    }

    [Fact]
    public void Constructor_WithNullNavigationManager_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new NavigationProvider(null!));
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
}