using KlinUtils.Blazor.Providers.Navigation;

using Microsoft.Extensions.DependencyInjection;

namespace KlinUtils.Blazor.Extensions;

public static class ProviderExtensions
{
    public static IServiceCollection AddNavigationProvider(this IServiceCollection services)
    {
        services.AddScoped<INavigationProvider, NavigationProvider>();

        return services;
    }
}