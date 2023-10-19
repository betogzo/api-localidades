using Localidades.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Localidades.Infrastructure.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Senha)
            .HasColumnName("senha")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(u => u.RegistradoEm)
            .HasColumnName("registrado_em")
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasMaxLength(30)
            .IsRequired();
    }
}

