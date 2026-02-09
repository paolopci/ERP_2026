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

        builder.Property(x => x.Quantita)
            .HasPrecision(18, 3)
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}
