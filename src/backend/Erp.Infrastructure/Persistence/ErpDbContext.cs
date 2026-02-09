using Erp.Domain.Inventory;
using Erp.Domain.MasterData;
using Erp.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace Erp.Infrastructure.Persistence;

public sealed class ErpDbContext : DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clienti => Set<Cliente>();

    public DbSet<Articolo> Articoli => Set<Articolo>();

    public DbSet<MovimentoMagazzino> MovimentiMagazzino => Set<MovimentoMagazzino>();

    public DbSet<DocumentoVendita> DocumentiVendita => Set<DocumentoVendita>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ErpDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
