using KlinUtils.Blazor.Providers.Navigation;
using KlinUtils.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace KlinUtils.Blazor.Extensions;

public static class ProviderExtensions
{
    public static IServiceCollection AddNavigationProvider(this IServiceCollection services)
    {
        services.AddScoped<INavigationProvider, NavigationProvider>();

        return services;
    }

    public static IServiceCollection AddNavigationProvider(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.AddServiceWithLifetime<INavigationProvider, NavigationProvider>(lifetime);

        return services;
    }
}