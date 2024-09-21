using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.common.Settings;
using ssptb.pe.tdlt.auth.common.Validations;
using ssptb.pe.tdlt.auth.dto.RolePermission;
using ssptb.pe.tdlt.auth.internalservices.Base;
using System.Net.Http.Json;

namespace ssptb.pe.tdlt.auth.internalservices.RolePermission;

/// <summary>
/// Servicio para obtener los permisos asociados a un rol.
/// </summary>
internal class RolePermissionService(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> settings, ILogger<RolePermissionService> logger, IBaseService baseService) : IRolePermissionService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IOptions<ApiSettings> _settings = settings;
    private readonly ILogger<RolePermissionService> _logger = logger;
    private readonly IBaseService _baseService = baseService;

    /// <summary>
    /// Obtiene los permisos asociados a un rol por RoleId.
    /// </summary>
    /// <param name="roleId">El ID del rol.</param>
    /// <returns>Lista de permisos asociados al rol.</returns>
    public async Task<ApiResponse<GetRolePermissionByRoleIdResponse>> GetPermissionsByRoleIdAsync(GetRolePermissionByRoleIdRequest request)
    {
        if (request.RoleId == Guid.Empty)
        {
            return ApiResponseHelper.CreateErrorResponse<GetRolePermissionByRoleIdResponse>("Invalid Role ID");
        }

        using HttpClient httpClient = _httpClientFactory.CreateClient("CustomClient");
        string path = GetPermissionsByRoleIdPath(request.RoleId);

        httpClient.BaseAddress = new Uri(_settings.Value.UrlMsUser);

        HttpResponseMessage httpResponse = await _baseService.GetAsync(httpClient, path);

        if (!CommonHttpValidation.ValidHttpResponse(httpResponse))
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogError("Error al consumir el servicio de obtener permisos por Role ID");
            _logger.LogError("Respuesta HTTP inválida: {StatusCode}, Contenido: {Content}", httpResponse.StatusCode, errorContent);
            return ApiResponseHelper.CreateErrorResponse<GetRolePermissionByRoleIdResponse>("Error al consumir el servicio de permisos");
        }

        var apiResult = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<GetRolePermissionByRoleIdResponse>>();

        return apiResult ?? ApiResponseHelper.CreateErrorResponse<GetRolePermissionByRoleIdResponse>("No se pudieron obtener los permisos.");
    }

    #region Private methods

    private static string GetPermissionsByRoleIdPath(Guid roleId)
    {
        return $"ssptbpetdlt/user/api/v1/RolePermission/{roleId}";
    }

    #endregion
}
