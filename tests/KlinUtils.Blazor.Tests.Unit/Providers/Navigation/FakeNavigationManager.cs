using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Tests.Unit.Providers.Navigation;

internal sealed class FakeNavigationManager : NavigationManager
{
    public FakeNavigationManager()
    {
        Initialize("https://example.com/", "https://example.com/");
    }

    public string? LastUri { get; private set; }

    public bool ForceLoadCalled { get; private set; }

    public bool ReplaceCalled { get; private set; }

    public NavigationOptions? UsedOptions { get; private set; }

    public void NavigateToDestination(string uri, NavigationOptions options)
    {
        LastUri = uri;
        UsedOptions = options;
        NavigateTo(uri, options);
    }

    public void NavigateToDestination(string uri, bool forceLoad, bool replace)
    {
        LastUri = uri;
        ForceLoadCalled = forceLoad;
        ReplaceCalled = replace;
        NavigateTo(uri, forceLoad, replace);
    }

    protected override void NavigateToCore([StringSyntax("Uri")] string uri, bool forceLoad)
    {
        NavigateToCore(uri, new NavigationOptions
        {
            ForceLoad = forceLoad,
        });
    }
}