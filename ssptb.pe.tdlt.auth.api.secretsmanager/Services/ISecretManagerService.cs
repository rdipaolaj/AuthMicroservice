using ssptb.pe.tdlt.auth.common.Secrets;

namespace ssptb.pe.tdlt.auth.api.secretsmanager.Services;
public interface ISecretManagerService
{
    Task<AuthSecrets?> GetAuthSecrets();
    Task<PostgresDbSecrets?> GetPostgresDbSecrets();
    Task<RedisSecrets?> GetRedisSecrets();
}
