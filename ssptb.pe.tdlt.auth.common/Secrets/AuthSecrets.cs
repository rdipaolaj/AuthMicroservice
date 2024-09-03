﻿using ssptb.pe.tdlt.auth.common.Settings;

namespace ssptb.pe.tdlt.auth.common.Secrets;
public class AuthSecrets : ISecret
{
    public string JwtSigningKey { get; set; } = string.Empty;
    //public string DatabaseConnectionString { get; set; } = string.Empty;
    public string OAuth2ClientSecret { get; set; } = string.Empty;
    public string ThirdPartyApiKey { get; set; } = string.Empty;
    public string SslCertificatePath { get; set; } = string.Empty;
    public string SslCertificatePassword { get; set; } = string.Empty;
}
