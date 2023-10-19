using Localidades.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Localidades.Infrastructure.Data.Mappings;

public class StateMapping : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("Estados");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
           .HasColumnName("Id")
           .ValueGeneratedOnAdd();

        builder.Property(x => x.CodigoUF)
           .HasColumnName("codigo_uf")
           .HasMaxLength(2);

        builder.Property(x => x.SiglaUF)
           .HasColumnName("sigla_uf")
           .HasMaxLength(2);

        builder.Property(x => x.NomeUF)
           .HasColumnName("nome_uf")
           .HasMaxLength(50);

        builder.Property(x => x.CriadoEm)
           .HasColumnName("criado_em")
           .HasColumnType("datetime")
           .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.ModificadoEm)
           .HasColumnName("modificado_em")
           .HasColumnType("datetime")
           .HasDefaultValueSql("GETDATE()");
    }
}

