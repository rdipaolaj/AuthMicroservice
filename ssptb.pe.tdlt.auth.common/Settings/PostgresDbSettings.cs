namespace ssptb.pe.tdlt.auth.common.Settings;
public class PostgresDbSettings
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Engine { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Dbname { get; set; } = string.Empty;

    public string DbInstanceIdentifier { get; set; } = string.Empty;
}
