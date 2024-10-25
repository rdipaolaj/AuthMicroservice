using MediatR;
using Microsoft.Extensions.Logging;
using ssptb.pe.tdlt.auth.command.Login;
using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.data.Validations.Auth.Service;
using ssptb.pe.tdlt.auth.data.Validations.Auth;
using ssptb.pe.tdlt.auth.dto.Login;
using ssptb.pe.tdlt.auth.dto.User.Requests;
using ssptb.pe.tdlt.auth.entities;
using ssptb.pe.tdlt.auth.internalservices.User;
using ssptb.pe.tdlt.auth.jwt.Services;
using ssptb.pe.tdlt.auth.redis.UserCache;
using ssptb.pe.tdlt.auth.dto.User;
using ssptb.pe.tdlt.auth.internalservices.RolePermission;
using ssptb.pe.tdlt.auth.dto.RolePermission;

namespace ssptb.pe.tdlt.auth.commandhandler.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<AuthUserResponse>>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IUserCacheService _userCacheService;
    private readonly IUserDataService _userDataService;
    private readonly IPasswordValidator _passwordValidator;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IRolePermissionService _rolePermissionService;

    public LoginCommandHandler(
        ILogger<LoginCommandHandler> logger,
        IUserCacheService userCacheService,
        IUserDataService userDataService,
        IPasswordValidator passwordValidator,
        IJwtTokenService jwtTokenService,
        IAuthUserRepository authUserRepository,
        IRolePermissionService rolePermissionService)
    {
        _logger = logger;
        _userCacheService = userCacheService;
        _userDataService = userDataService;
        _passwordValidator = passwordValidator;
        _jwtTokenService = jwtTokenService;
        _authUserRepository = authUserRepository;
        _rolePermissionService = rolePermissionService;
    }

    public async Task<ApiResponse<AuthUserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to authenticate user...");

        var user = await GetUserFromCacheOrServiceAsync(request.Username);

        if (user == null)
        {
            return ApiResponseHelper.CreateErrorResponse<AuthUserResponse>("Invalid username or password");
        }

        if (!ValidatePassword(request.Password, user.HashedPassword, user.SaltPassword))
        {
            return ApiResponseHelper.CreateErrorResponse<AuthUserResponse>("Invalid username or password");
        }

        var jwtToken = _jwtTokenService.GenerateToken(user);

        var authUser = await UpdateAuthUserAsync(user.Username, jwtToken.Token, jwtToken.Expiry, cancellationToken);

        await UpdateUserCacheAsync(user, authUser.LastLogin);

        var response = PrepareResponse(authUser, user.Permissions, user.UserId, user.RoleId);

        _logger.LogInformation("Usuario autenticado exitosamente: {Username}", request.Username);
        return ApiResponseHelper.CreateSuccessResponse(response, "User authenticated successfully");
    }

    private async Task<GetUserByUsernameResponse> GetUserFromCacheOrServiceAsync(string username)
    {
        var user = await _userCacheService.GetUserByUsernameAsync(username);
        if (user == null)
        {
            _logger.LogInformation("Usuario no encontrado en Redis, buscando en el microservicio de usuario...");

            var userResponse = await _userDataService.GetUserDataClient(new GetUserByUsernameRequest
            {
                Username = username
            });

            if (userResponse.Success)
            {
                user = userResponse.Data;

                // Obtener los permisos del usuario basado en su RoleId usando el nuevo método
                user.Permissions = await GetPermissionsByRoleIdAsync(user.RoleId);

                await _userCacheService.SaveUserAsync(user, TimeSpan.FromHours(1));
            }
        }
        else
        {
            // Si el usuario está en el caché, obtener los permisos también
            user.Permissions = await GetPermissionsByRoleIdAsync(user.RoleId);
        }

        return user;
    }

    // Método reutilizable para obtener los permisos de un RoleId
    private async Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(Guid roleId)
    {
        var permissionsResponse = await _rolePermissionService.GetPermissionsByRoleIdAsync(new GetRolePermissionByRoleIdRequest
        {
            RoleId = roleId
        });

        if (permissionsResponse.Success)
        {
            return permissionsResponse.Data.Permissions;
        }

        _logger.LogWarning($"No se pudieron obtener los permisos para el Role ID {roleId}");
        return new List<PermissionDto>(); // Retornar una lista vacía si no se pueden obtener los permisos
    }

    private bool ValidatePassword(string inputPassword, string storedHashedPassword, string salt)
    {
        return _passwordValidator.ValidatePassword(inputPassword, storedHashedPassword, salt);
    }

    private async Task<AuthUser> UpdateAuthUserAsync(string username, string token, DateTime tokenExpiry, CancellationToken cancellationToken)
    {
        var authUser = new AuthUser
        {
            Username = username,
            JwtToken = token,
            TokenExpiry = tokenExpiry,
            LastLogin = DateTime.UtcNow
        };

        await _authUserRepository.AddAuthUserAsync(authUser);
        await _authUserRepository.SaveChangesAsync(cancellationToken);

        return authUser;
    }

    private async Task UpdateUserCacheAsync(GetUserByUsernameResponse user, DateTime lastLogin)
    {
        user.LastLogin = lastLogin;
        await _userCacheService.SaveUserAsync(user, TimeSpan.FromHours(1));
    }

    private AuthUserResponse PrepareResponse(AuthUser authUser, List<PermissionDto> permissions, Guid userId, Guid rolId)
    {
        return new AuthUserResponse
        {
            Username = authUser.Username,
            UserId = userId,
            RolId = rolId,
            JwtToken = authUser.JwtToken,
            TokenExpiry = authUser.TokenExpiry,
            Permissions = permissions // Incluye los permisos en la respuesta
        };
    }
}