using Konscious.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Logging;
using ssptb.pe.tdlt.auth.command.Login;
using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.data;
using ssptb.pe.tdlt.auth.dto.Login;
using ssptb.pe.tdlt.auth.dto.User;
using ssptb.pe.tdlt.auth.dto.User.Requests;
using ssptb.pe.tdlt.auth.entities;
using ssptb.pe.tdlt.auth.internalservices.User;
using ssptb.pe.tdlt.auth.jwt.Services;
using ssptb.pe.tdlt.auth.redis.Services;
using System.Text;

namespace ssptb.pe.tdlt.auth.commandhandler.Login;
public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<AuthUserResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IRedisService _redisService;
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUserDataService _userDataService;

    public LoginCommandHandler(ApplicationDbContext context, IRedisService redisService, ILogger<LoginCommandHandler> logger, IJwtTokenGenerator tokenGenerator, IUserDataService userDataService)
    {
        _context = context;
        _redisService = redisService;
        _logger = logger;
        _tokenGenerator = tokenGenerator;
        _userDataService = userDataService;
    }

    public async Task<ApiResponse<AuthUserResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to authenticate user...");

        // Buscar el usuario en Redis
        var redisKey = $"user:{request.Username}";
        var user = await _redisService.GetInformationAsync<GetUserByUsernameResponse>(redisKey);

        if (user == null)
        {
            _logger.LogInformation("Usuario no encontrado en Redis, buscando en el microservicio de usuario...");

            // Crear el request para el servicio de usuario
            var getUserRequest = new GetUserByUsernameRequest
            {
                Username = request.Username
            };

            // Llamar al microservicio de usuario si no está en Redis
            var userResponse = await _userDataService.GetUserDataClient(getUserRequest);

            if (!userResponse.Success)
            {
                return ApiResponseHelper.CreateErrorResponse<AuthUserResponse>("Invalid username or password");
            }

            user = new GetUserByUsernameResponse
            {
                UserId = userResponse.Data.UserId,
                Username = userResponse.Data.Username,
                HashedPassword = userResponse.Data.HashedPassword,
                SaltPassword = userResponse.Data.SaltPassword,
                Email = userResponse.Data.Email,
                PhoneNumber = userResponse.Data.PhoneNumber,
                UserRole = userResponse.Data.UserRole,
                CompanyName = userResponse.Data.CompanyName,
                Department = userResponse.Data.Department,
                JobTitle = userResponse.Data.JobTitle,
                CreatedAt = userResponse.Data.CreatedAt,
                LastLogin = userResponse.Data.LastLogin,
                AccountStatus = userResponse.Data.AccountStatus
            };

            // Guardar la información del usuario en Redis para futuras solicitudes
            await _redisService.SaveInformationAsJsonAsync(redisKey, user, TimeSpan.FromHours(1));
        }

        AuthUser authUser = new AuthUser();

        // Validar la contraseña usando Argon2
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(request.Password))
        {
            Salt = Convert.FromBase64String(user.SaltPassword), // El salt debería estar almacenado junto con la contraseña
            DegreeOfParallelism = 8,
            MemorySize = 8192,
            Iterations = 4
        };

        var hashedInputPassword = Convert.ToBase64String(argon2.GetBytes(16));
        if (hashedInputPassword != user.HashedPassword)
        {
            return ApiResponseHelper.CreateErrorResponse<AuthUserResponse>("Invalid username or password");
        }

        // Generar el token JWT
        var jwtToken = _tokenGenerator.GenerateToken(user);

        // Actualizar la última fecha de acceso y almacenar el token JWT
        authUser.LastLogin = DateTime.UtcNow;
        authUser.Username = user.Username;
        authUser.JwtToken = jwtToken;
        authUser.TokenExpiry = DateTime.UtcNow.AddHours(1); // El token expira en 1 hora

        _context.AuthUsers.Add(authUser);
        await _context.SaveChangesAsync(cancellationToken);

        // Guardar la información actualizada en Redis
        await _redisService.SaveInformationAsJsonAsync(redisKey, user, TimeSpan.FromHours(1));

        // Preparar la respuesta
        var response = new AuthUserResponse
        {
            Username = authUser.Username,
            JwtToken = jwtToken,
            TokenExpiry = authUser.TokenExpiry
        };

        return ApiResponseHelper.CreateSuccessResponse(response, "User authenticated successfully");
    }
}