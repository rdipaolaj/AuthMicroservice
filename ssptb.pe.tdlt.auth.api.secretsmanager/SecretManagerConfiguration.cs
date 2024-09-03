using Microsoft.Extensions.DependencyInjection;
using ssptb.pe.tdlt.auth.api.secretsmanager.Services;

namespace ssptb.pe.tdlt.auth.api.secretsmanager;
public static class SecretManagerConfiguration
{
    public static IServiceCollection AddSecretManagerConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<ISecretManagerService, SecretManagerService>();

        return services;
    }
}
