using KlinUtils.Common.Helpers;

using Microsoft.Extensions.DependencyInjection;

namespace KlinUtils.Common.Extensions;

public static class HelperExtensions
{
    public static IServiceCollection AddPasswordHelper(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHelper, PasswordHelper>();

        return services;
    }

    public static IServiceCollection AddPasswordHelper(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.AddServiceWithLifetime<IPasswordHelper, PasswordHelper>(lifetime);

        return services;
    }

    public static void AddServiceWithLifetime<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<TService, TImplementation>();
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped<TService, TImplementation>();
                break;
            case ServiceLifetime.Transient:
                services.AddTransient<TService, TImplementation>();
                break;
            default:
                break;
        }
    }
}