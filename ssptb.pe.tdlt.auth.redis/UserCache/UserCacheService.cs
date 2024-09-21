using ssptb.pe.tdlt.auth.dto.User;
using ssptb.pe.tdlt.auth.redis.Services;

namespace ssptb.pe.tdlt.auth.redis.UserCache;
public class UserCacheService : IUserCacheService
{
    private readonly IRedisService _redisService;

    public UserCacheService(IRedisService redisService)
    {
        _redisService = redisService;
    }

    public async Task<GetUserByUsernameResponse> GetUserByUsernameAsync(string username)
    {
        var redisKey = $"user:{username}";
        return await _redisService.GetInformationAsync<GetUserByUsernameResponse>(redisKey);
    }

    public async Task SaveUserAsync(GetUserByUsernameResponse user, TimeSpan expiration)
    {
        var redisKey = $"user:{user.Username}";
        await _redisService.SaveInformationAsJsonAsync(redisKey, user, expiration);
    }
}
