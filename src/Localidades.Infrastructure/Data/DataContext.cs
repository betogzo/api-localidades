using Localidades.Domain.Models;
using Localidades.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Localidades.Infrastructure.Data;

public class DataContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Estado> Estados { get; set; }
    public DbSet<Municipio> Municipios { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new StateMapping());
        modelBuilder.ApplyConfiguration(new CityMapping());
    }
}