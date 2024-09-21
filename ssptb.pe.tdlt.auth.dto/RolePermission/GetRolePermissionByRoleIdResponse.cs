namespace ssptb.pe.tdlt.auth.dto.RolePermission;
public class GetRolePermissionByRoleIdResponse
{
    public Guid RoleId { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
}

public class PermissionDto
{
    public Guid PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
