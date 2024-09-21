using ssptb.pe.tdlt.auth.entities;

namespace ssptb.pe.tdlt.auth.data.Validations.Auth.Service;
public interface IAuthUserRepository
{
    Task AddAuthUserAsync(AuthUser authUser);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
