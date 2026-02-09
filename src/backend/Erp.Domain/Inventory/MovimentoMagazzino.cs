using Erp.Domain.Common;

namespace Erp.Domain.Inventory;

public sealed class MovimentoMagazzino : AggregateRoot
{
    public MovimentoMagazzino(Guid id, Guid articoloId, Guid sedeId, decimal quantita, TipoMovimentoMagazzino tipo)
        : base(id)
    {
        ArticoloId = articoloId;
        SedeId = sedeId;
        Quantita = quantita;
        Tipo = tipo;
    }

    public Guid ArticoloId { get; private set; }

    public Guid SedeId { get; private set; }

    public decimal Quantita { get; private set; }

    public TipoMovimentoMagazzino Tipo { get; private set; }
}

public enum TipoMovimentoMagazzino
{
    Entrata = 0,
    Uscita = 1,
    Rettifica = 2
}
