using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Providers.Navigation;

internal sealed class NavigationProvider(NavigationManager navigationManager) : INavigationProvider
{
    private readonly NavigationManager _navigationManager =
        navigationManager ?? throw new ArgumentNullException();

    public string BaseUrl => _navigationManager.BaseUri;

    public string ToAbsoluteUrl(string relativeUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativeUrl);

        return string.Concat(BaseUrl, relativeUrl.AsSpan().TrimStart('/'));
    }

    public void NavigateTo(string route, bool isInternalUrl = true)
    {
        NavigateTo(route, isInternalUrl, false, false);
    }

    public void NavigateTo(string route, NavigationOptions navigationOptions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(route);

        _navigationManager.NavigateTo(route, navigationOptions);
    }

    public void NavigateTo(string route, bool isInternalUrl = true, bool forceLoad = false, bool replace = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(route);

        if (isInternalUrl)
        {
            _navigationManager.NavigateTo(ToAbsoluteUrl(route), forceLoad, replace);
        }
        else
        {
            _navigationManager.NavigateTo(route, forceLoad, replace);
        }
    }
}