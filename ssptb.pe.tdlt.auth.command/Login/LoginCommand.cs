using MediatR;
using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.dto.Login;

namespace ssptb.pe.tdlt.auth.command.Login;
public class LoginCommand : IRequest<ApiResponse<AuthUserResponse>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Contraseña en texto plano que será validada
}
