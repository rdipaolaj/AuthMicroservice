using ssptb.pe.tdlt.auth.dto.User;
using ssptb.pe.tdlt.auth.entities;

namespace ssptb.pe.tdlt.auth.jwt.Services;
public interface IJwtTokenGenerator
{
    string GenerateToken(GetUserByUsernameResponse user);
}
