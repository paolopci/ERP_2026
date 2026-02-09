using Erp.Domain.Common;

namespace Erp.Domain.MasterData;

public sealed class Articolo : AggregateRoot
{
    public Articolo(Guid id, string sku, string descrizione, string unitaMisura, bool attivo)
        : base(id)
    {
        Sku = sku;
        Descrizione = descrizione;
        UnitaMisura = unitaMisura;
        Attivo = attivo;
    }

    public string Sku { get; private set; }

    public string Descrizione { get; private set; }

    public string UnitaMisura { get; private set; }

    public bool Attivo { get; private set; }
}
