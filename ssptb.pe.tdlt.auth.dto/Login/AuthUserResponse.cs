namespace ssptb.pe.tdlt.auth.dto.Login;
public class AuthUserResponse
{
    public string Username { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;
    public DateTime TokenExpiry { get; set; }
    // Puedes agregar más campos si es necesario
}