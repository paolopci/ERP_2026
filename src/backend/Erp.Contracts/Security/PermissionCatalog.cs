namespace Erp.Contracts.Security;

public static class PermissionCatalog
{
    public const string AnagraficheRead = "anagrafiche.read";
    public const string AnagraficheWrite = "anagrafiche.write";
    public const string ArticoliRead = "articoli.read";
    public const string ArticoliWrite = "articoli.write";
    public const string MagazzinoRead = "magazzino.read";
    public const string MagazzinoMovimenta = "magazzino.movimenta";
    public const string MagazzinoRettifica = "magazzino.rettifica";
    public const string VenditeRead = "vendite.read";
    public const string VenditeWrite = "vendite.write";
    public const string VenditeEmettiDocumento = "vendite.emetti-documento";
    public const string AuditRead = "audit.read";
    public const string Administration = "administration.full";

    public static IReadOnlyList<string> All { get; } =
    [
        AnagraficheRead,
        AnagraficheWrite,
        ArticoliRead,
        ArticoliWrite,
        MagazzinoRead,
        MagazzinoMovimenta,
        MagazzinoRettifica,
        VenditeRead,
        VenditeWrite,
        VenditeEmettiDocumento,
        AuditRead,
        Administration
    ];
}
