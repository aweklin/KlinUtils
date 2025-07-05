using Microsoft.AspNetCore.Components;

namespace KlinUtils.Blazor.Providers.Navigation;

public interface INavigationProvider
{
    /// <summary>
    /// Gets the current base URI with trailing slash.
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Returns the base url with the provided relative path.
    /// </summary>
    /// <param name="relativeUrl">Specifies the relative path to be added to the base url.</param>
    /// <returns>System.String.</returns>
    string ToAbsoluteUrl(string relativeUrl);

    /// <summary>
    /// Navigates to the specified URI.
    /// </summary>
    /// <param name="route">Specifies the endpoint you want to navigate to.</param>
    /// <param name="isInternalUrl">Specifies if the navigation is withing your application so as to prefix the base url to the route specified.</param>
    void NavigateTo(string route, bool isInternalUrl);

    /// <summary>
    /// Navigates to the specified URI.
    /// </summary>
    /// <param name="route">Specifies the endpoint you want to navigate to.</param>
    /// <param name="navigationOptions">Provides additional Microsoft.AspNetCore.Components.NavigationOptions.</param>
    void NavigateTo(string route, NavigationOptions navigationOptions);

    /// <summary>
    /// Navigates to the specified URI.
    /// </summary>
    /// <param name="route">Specifies the endpoint you want to navigate to.</param>
    /// <param name="isInternalUrl">Specifies if the navigation is withing your application so as to prefix the base url to the route specified.</param>
    /// <param name="forceLoad">If true, bypasses client-side routing and forces the browser to load the new page from the server, whether or not the URI would normally be handled by the client-side router.</param>
    /// <param name="replace">If true, replaces the current entry in the history stack. If false, appends the new entry to the history stack.</param>
    void NavigateTo(string route, bool isInternalUrl = true, bool forceLoad = false, bool replace = false);
}