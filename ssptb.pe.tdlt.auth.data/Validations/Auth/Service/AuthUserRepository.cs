using ssptb.pe.tdlt.auth.entities;

namespace ssptb.pe.tdlt.auth.data.Validations.Auth.Service;
public class AuthUserRepository : IAuthUserRepository
{
    private readonly ApplicationDbContext _context;

    public AuthUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAuthUserAsync(AuthUser authUser)
    {
        _context.AuthUsers.Add(authUser);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

