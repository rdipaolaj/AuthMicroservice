namespace ssptb.pe.tdlt.auth.common.Validations;
public static class CommonHttpValidation
{
    public static bool ValidHttpResponse(HttpResponseMessage response)
        => response != null && response.IsSuccessStatusCode && response.Content != null;
}