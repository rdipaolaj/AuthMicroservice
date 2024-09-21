using Microsoft.Extensions.DependencyInjection;
using ssptb.pe.tdlt.auth.jwt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssptb.pe.tdlt.auth.jwt;

/// <summary>
/// Métodos de extensión para configuración de jwt
/// </summary>
public static class JwtServiceConfiguration
{
    /// <summary>
    /// Configuración de servicio jwt
    /// </summary>
    /// <param name="services"></param>
    /// <returns>Retorna service collection para que funcione como método de extensión</returns>
    public static IServiceCollection AddJWTServiceConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
