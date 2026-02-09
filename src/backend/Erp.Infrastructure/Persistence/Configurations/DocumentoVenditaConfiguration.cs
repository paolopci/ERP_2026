using Erp.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Erp.Infrastructure.Persistence.Configurations;

public sealed class DocumentoVenditaConfiguration : IEntityTypeConfiguration<DocumentoVendita>
{
    public void Configure(EntityTypeBuilder<DocumentoVendita> builder)
    {
        builder.ToTable("DocumentiVendita", "sales");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId).IsRequired();

        builder.Property(x => x.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Stato)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();

        builder.HasIndex(x => new { x.CompanyId, x.ClienteId });
    }
}
