using KlinUtils.Blazor.Providers.Navigation;
using KlinUtils.Blazor.Providers.Storage;
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

    public static IServiceCollection AddStorageProvider(this IServiceCollection services)
    {
        services.AddScoped<IStorageProvider, StorageProvider>();

        return services;
    }

    public static IServiceCollection AddStorageProvider(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.AddServiceWithLifetime<IStorageProvider, StorageProvider>(lifetime);

        return services;
    }
}