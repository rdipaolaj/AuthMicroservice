using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.common.Settings;
using ssptb.pe.tdlt.auth.common.Validations;
using ssptb.pe.tdlt.auth.dto.User;
using ssptb.pe.tdlt.auth.dto.User.Requests;
using ssptb.pe.tdlt.auth.internalservices.Base;
using System.Net.Http.Json;

namespace ssptb.pe.tdlt.auth.internalservices.User;

/// <summary>
/// Clase de ms servicio de user data
/// </summary>
/// <param name="httpClientFactory">Inyección de dependencias de HttpClienteFactory</param>
/// <param name="settings">Inyección de dependecias de settigs</param>
internal class UserDataService(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> settings, ILogger<UserDataService> logger, IBaseService baseService) : IUserDataService
{
    /// <summary>
    /// Intefaz de HttpClientFactory para el uso de HttpClients
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    /// <summary>
    /// Settings para obtener Urls
    /// </summary>
    private readonly IOptions<ApiSettings> _settings = settings;

    private readonly ILogger<UserDataService> _logger = logger;

    private readonly IBaseService _baseService = baseService;

    /// <summary>
    /// Llamada a servicio para obtener la información del cliente
    /// </summary>
    /// <param name="request">Request para consulta de cliente</param>
    /// <returns>Información de cliente</returns>
    public async Task<ApiResponse<GetUserByUsernameResponse>> GetUserDataClient(GetUserByUsernameRequest request)
    {
        if (string.IsNullOrEmpty(request.Username))
            return ApiResponseHelper.CreateErrorResponse<GetUserByUsernameResponse>("Invalid username");

        using HttpClient httpClient = _httpClientFactory.CreateClient("CustomClient");
        string path = GetUserByUsernamePath(request.Username);

        httpClient.BaseAddress = new Uri(_settings.Value.UrlMsUser);

        HttpResponseMessage httpResponse = await _baseService.GetAsync(httpClient, path);

        if (!CommonHttpValidation.ValidHttpResponse(httpResponse))
        {
            var errorContent = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogError("Error al consumir el servicio de obtener clientes");
            _logger.LogError("Respuesta HTTP inválida: {StatusCode}, Contenido: {Content}", httpResponse.StatusCode, errorContent);
            return ApiResponseHelper.CreateErrorResponse<GetUserByUsernameResponse>("Error al consumir el servicio");
        }

        var apiResult = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<GetUserByUsernameResponse>>();

        return apiResult;
    }

    #region Private methods

    private static string GetUserByUsernamePath(string username)
    {
        return $"ssptbpetdlt/user/api/v1/User/username/{username}";
    }

    #endregion
}
