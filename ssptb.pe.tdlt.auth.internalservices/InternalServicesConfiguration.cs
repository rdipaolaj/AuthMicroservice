using Microsoft.Extensions.DependencyInjection;
using ssptb.pe.tdlt.auth.internalservices.Base;
using ssptb.pe.tdlt.auth.internalservices.User;

namespace ssptb.pe.tdlt.auth.internalservices;
public static class InternalServicesConfiguration
{
    public static IServiceCollection AddInternalServicesConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IBaseService, BaseService>();
        services.AddTransient<IUserDataService, UserDataService>();

        return services;
    }
}
