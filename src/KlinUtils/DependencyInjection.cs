using KlinUtils.Networking;

using Microsoft.Extensions.DependencyInjection;

namespace KlinUtils;

public static class DependencyInjection
{
    public static IServiceCollection AddNetworkClient(this IServiceCollection services)
    {
        services.AddSingleton<INetworkClient, NetworkClient>();

        return services;
    }
}