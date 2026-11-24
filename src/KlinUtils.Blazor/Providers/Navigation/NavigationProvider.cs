using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KlinUtils.Blazor.Providers.Navigation;

internal sealed class NavigationProvider(
    NavigationManager navigationManager,
    IJSRuntime jSRuntime) : INavigationProvider
{
    private readonly NavigationManager _navigationManager =
        navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));

    private readonly IJSRuntime _jSRuntime =
        jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));

    public string BaseUrl => _navigationManager.BaseUri;

    public string Uri => _navigationManager.Uri;

    public ReadOnlySpan<char> RelativePath => _navigationManager.ToBaseRelativePath(_navigationManager.Uri);

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

    public ValueTask BackAsync(CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync("history.back", cancellationToken);
    }

    public ValueTask ForwardAsync(CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync("history.forward", cancellationToken);
    }

    public ValueTask GotoAsync(int delta, CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync("history.go", cancellationToken, delta);
    }
}