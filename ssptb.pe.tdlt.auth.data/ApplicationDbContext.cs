using Microsoft.EntityFrameworkCore;
using ssptb.pe.tdlt.auth.entities;

namespace ssptb.pe.tdlt.auth.data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Especifica el esquema predeterminado
        modelBuilder.HasDefaultSchema("authdb");

        base.OnModelCreating(modelBuilder);
    }

    // Definir DbSets
    public DbSet<AuthUser> AuthUsers { get; set; }
}
