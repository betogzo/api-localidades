using Localidades.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Localidades.Infrastructure.Data.Mappings;

public class CityMapping : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.ToTable("Municipios");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
           .HasColumnName("Id")
           .ValueGeneratedOnAdd();

        builder.Property(x => x.CodigoIBGE)
           .HasColumnName("codigo_municipio")
           .HasMaxLength(7);

        builder.Property(x => x.CodigoUF)
           .HasColumnName("codigo_uf")
           .HasMaxLength(2);

        builder.Property(x => x.NomeMunicipio)
           .HasColumnName("nome_municipio")
           .HasMaxLength(80);

        builder.Property(x => x.CriadoEm)
           .HasColumnName("criado_em")
           .HasColumnType("datetime")
           .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.ModificadoEm)
           .HasColumnName("modificado_em")
           .HasColumnType("datetime")
           .HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.Estado)
            .WithMany(x => x.Municipios)
            .HasConstraintName("FK_Municipios_Estados")
            .HasForeignKey(x => x.CodigoUF)
            .HasPrincipalKey(x => x.CodigoUF)
            .OnDelete(DeleteBehavior.Cascade);
    }
}