using ssptb.pe.tdlt.auth.dto.User;

namespace ssptb.pe.tdlt.auth.redis.UserCache;
public interface IUserCacheService
{
    Task<GetUserByUsernameResponse> GetUserByUsernameAsync(string username);
    Task SaveUserAsync(GetUserByUsernameResponse user, TimeSpan expiration);
}
