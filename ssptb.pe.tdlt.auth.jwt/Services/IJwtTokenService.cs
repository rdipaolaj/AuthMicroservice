using ssptb.pe.tdlt.auth.dto.User;

namespace ssptb.pe.tdlt.auth.jwt.Services;
public interface IJwtTokenService
{
    (string Token, DateTime Expiry) GenerateToken(GetUserByUsernameResponse user);
}
