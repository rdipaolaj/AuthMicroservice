using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.dto.RolePermission;

namespace ssptb.pe.tdlt.auth.internalservices.RolePermission;
public interface IRolePermissionService
{
    Task<ApiResponse<GetRolePermissionByRoleIdResponse>> GetPermissionsByRoleIdAsync(GetRolePermissionByRoleIdRequest request);
}
