using Erp.Domain.Common;

namespace Erp.Domain.MasterData;

public sealed class Cliente : AggregateRoot
{
    public Cliente(Guid id, string codice, string ragioneSociale, string email, bool attivo)
        : base(id)
    {
        Codice = codice;
        RagioneSociale = ragioneSociale;
        Email = email;
        Attivo = attivo;
    }

    public string Codice { get; private set; }

    public string RagioneSociale { get; private set; }

    public string Email { get; private set; }

    public bool Attivo { get; private set; }

    public void AggiornaContatti(string email)
    {
        Email = email;
        Touch();
    }

    public void Disattiva()
    {
        Attivo = false;
        Touch();
    }
}
