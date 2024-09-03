using Microsoft.EntityFrameworkCore;
using ssptb.pe.tdlt.auth.entities;

namespace ssptb.pe.tdlt.auth.data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Definir DbSets
    public DbSet<AuthUser> AuthUsers { get; set; }
}
