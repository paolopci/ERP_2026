using Erp.Domain.MasterData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Infrastructure.Persistence.Configurations;

public sealed class ArticoloConfiguration : IEntityTypeConfiguration<Articolo>
{
    public void Configure(EntityTypeBuilder<Articolo> builder)
    {
        builder.ToTable("Articoli", "masterdata");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Sku)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CompanyId).IsRequired();

        builder.Property(x => x.Descrizione)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.UnitaMisura)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Attivo)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => new { x.CompanyId, x.Sku }).IsUnique();
    }
}
