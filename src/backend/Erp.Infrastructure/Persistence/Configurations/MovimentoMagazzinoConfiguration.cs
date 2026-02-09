using Erp.Domain.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Infrastructure.Persistence.Configurations;

public sealed class MovimentoMagazzinoConfiguration : IEntityTypeConfiguration<MovimentoMagazzino>
{
    public void Configure(EntityTypeBuilder<MovimentoMagazzino> builder)
    {
        builder.ToTable("MovimentiMagazzino", "inventory");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId).IsRequired();

        builder.Property(x => x.Quantita)
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => new { x.CompanyId, x.ArticoloId, x.SedeId });
    }
}
