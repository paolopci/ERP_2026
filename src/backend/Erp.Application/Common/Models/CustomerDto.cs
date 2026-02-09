namespace Erp.Application.Common.Models;

public sealed record CustomerDto(
    Guid Id,
    string Codice,
    string RagioneSociale,
    string Email,
    bool Attivo);
