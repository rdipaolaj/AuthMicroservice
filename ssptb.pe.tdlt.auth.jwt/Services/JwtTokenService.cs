using ssptb.pe.tdlt.auth.dto.User;

namespace ssptb.pe.tdlt.auth.jwt.Services;
public class JwtTokenService : IJwtTokenService
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    public JwtTokenService(IJwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
    }

    public (string Token, DateTime Expiry) GenerateToken(GetUserByUsernameResponse user)
    {
        var token = _tokenGenerator.GenerateToken(user);
        return (token, DateTime.UtcNow.AddHours(1)); // Token válido por 1 hora
    }
}
