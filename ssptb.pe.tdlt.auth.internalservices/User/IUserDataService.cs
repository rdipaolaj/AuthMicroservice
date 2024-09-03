using ssptb.pe.tdlt.auth.common.Responses;
using ssptb.pe.tdlt.auth.dto.User;
using ssptb.pe.tdlt.auth.dto.User.Requests;

namespace ssptb.pe.tdlt.auth.internalservices.User;

/// <summary>
/// Interfaz de ms servicio de user data
/// </summary>
public interface IUserDataService
{
    /// <summary>
    /// Llamada a servicio para obtener la información del cliente
    /// </summary>
    /// <param name="request">Request para consulta de cliente</param>
    /// <returns>Información de cliente</returns>
    Task<ApiResponse<GetUserByUsernameResponse>> GetUserDataClient(GetUserByUsernameRequest request);
}
