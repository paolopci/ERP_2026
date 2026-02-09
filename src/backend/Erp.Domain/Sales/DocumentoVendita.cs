using Erp.Domain.Common;

namespace Erp.Domain.Sales;

public sealed class DocumentoVendita : AggregateRoot
{
    public DocumentoVendita(Guid id, Guid companyId, Guid clienteId, TipoDocumentoVendita tipo)
        : base(id)
    {
        CompanyId = companyId;
        ClienteId = clienteId;
        Tipo = tipo;
        Stato = StatoDocumentoVendita.Bozza;
    }

    public Guid CompanyId { get; private set; }

    public Guid ClienteId { get; private set; }

    public TipoDocumentoVendita Tipo { get; private set; }

    public StatoDocumentoVendita Stato { get; private set; }

    public void Conferma()
    {
        Stato = StatoDocumentoVendita.Confermato;
        Touch();
    }
}

public enum TipoDocumentoVendita
{
    Preventivo = 0,
    Ordine = 1,
    Ddt = 2,
    Fattura = 3
}

public enum StatoDocumentoVendita
{
    Bozza = 0,
    Confermato = 1,
    Emesso = 2
}
