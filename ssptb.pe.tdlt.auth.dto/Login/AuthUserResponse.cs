using ssptb.pe.tdlt.auth.dto.RolePermission;

namespace ssptb.pe.tdlt.auth.dto.Login;
public class AuthUserResponse
{
    public string Username { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid RolId { get; set; }
    public string JwtToken { get; set; } = string.Empty;
    public DateTime TokenExpiry { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    // Puedes agregar más campos si es necesario
}