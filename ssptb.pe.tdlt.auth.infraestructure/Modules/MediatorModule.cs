using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ssptb.pe.tdlt.auth.api.secretsmanager;
using ssptb.pe.tdlt.auth.redis;
using ssptb.pe.tdlt.auth.infraestructure.Behaviors;
using ssptb.pe.tdlt.auth.internalservices;
using ssptb.pe.tdlt.auth.jwt;
using ssptb.pe.tdlt.auth.commandhandler.Login;

namespace ssptb.pe.tdlt.auth.infraestructure.Modules;
public static class MediatorModule
{
    public static IServiceCollection AddMediatRAssemblyConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining(typeof(LoginCommandHandler));

            configuration.AddOpenBehavior(typeof(ValidatorBehavior<,>));
        });

        //services.AddValidatorsFromAssembly(typeof(AffiliateCommandValidator).Assembly);

        return services;
    }
    public static IServiceCollection AddCustomServicesConfiguration(this IServiceCollection services)
    {
        services.AddInternalServicesConfiguration();
        services.AddSecretManagerConfiguration();
        services.AddRedisServiceConfiguration();
        services.AddJWTServiceConfiguration();

        return services;
    }
}
