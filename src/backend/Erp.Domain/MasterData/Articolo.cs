using Erp.Domain.Common;

namespace Erp.Domain.MasterData;

public sealed class Articolo : AggregateRoot
{
    public Articolo(Guid id, Guid companyId, string sku, string descrizione, string unitaMisura, bool attivo)
        : base(id)
    {
        CompanyId = companyId;
        Sku = sku;
        Descrizione = descrizione;
        UnitaMisura = unitaMisura;
        Attivo = attivo;
    }

    public Guid CompanyId { get; private set; }

    public string Sku { get; private set; }

    public string Descrizione { get; private set; }

    public string UnitaMisura { get; private set; }

    public bool Attivo { get; private set; }
}
