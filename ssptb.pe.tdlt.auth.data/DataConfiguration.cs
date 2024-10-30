using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ssptb.pe.tdlt.auth.common.Settings;
using ssptb.pe.tdlt.auth.data.Validations.Auth;
using ssptb.pe.tdlt.auth.data.Validations.Auth.Service;

namespace ssptb.pe.tdlt.auth.data;
public static class DataConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Configura la cadena de conexión obtenida de los secretos de AWS o directamente de la configuración
        var serviceProvider = services.BuildServiceProvider();
        var postgresSettings = serviceProvider.GetService<IOptions<PostgresDbSettings>>()?.Value;

        if (postgresSettings == null)
        {
            throw new InvalidOperationException("PostgresDbSettings not configured properly.");
        }

        var connectionString = $"Host={postgresSettings.Host};Port={postgresSettings.Port};Username={postgresSettings.Username};Password={postgresSettings.Password};Database={postgresSettings.Dbname};";

        // Configura el DbContext con la cadena de conexión de PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.CommandTimeout(30); // Ajusta el tiempo de espera si es necesario
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "authdb"); // Forzar la tabla de migración en `authdb`
            })
        );

        return services;
    }

    public static IServiceCollection AddDataServicesConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IPasswordValidator, Argon2PasswordValidator>();
        services.AddScoped<IAuthUserRepository, AuthUserRepository>();

        return services;
    }
}
