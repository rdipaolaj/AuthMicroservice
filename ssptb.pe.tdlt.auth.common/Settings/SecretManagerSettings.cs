namespace ssptb.pe.tdlt.auth.common.Settings;
public class SecretManagerSettings
{
    public bool Local { get; set; }
    public string Region { get; set; } = string.Empty;
    public string ArnAuthSecrets { get; set; } = string.Empty;
    public string ArnPostgresSecrets { get; set; } = string.Empty;
}
