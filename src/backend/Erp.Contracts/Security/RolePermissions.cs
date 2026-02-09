namespace Erp.Contracts.Security;

public static class RolePermissions
{
    public static IReadOnlyDictionary<Role, IReadOnlySet<string>> Matrix { get; } =
        new Dictionary<Role, IReadOnlySet<string>>
        {
            [Role.Admin] = new HashSet<string>(PermissionCatalog.All),
            [Role.Contabile] = new HashSet<string>
            {
                PermissionCatalog.AnagraficheRead,
                PermissionCatalog.ArticoliRead,
                PermissionCatalog.VenditeRead,
                PermissionCatalog.VenditeWrite,
                PermissionCatalog.VenditeEmettiDocumento,
                PermissionCatalog.AuditRead
            },
            [Role.Magazziniere] = new HashSet<string>
            {
                PermissionCatalog.AnagraficheRead,
                PermissionCatalog.ArticoliRead,
                PermissionCatalog.MagazzinoRead,
                PermissionCatalog.MagazzinoMovimenta,
                PermissionCatalog.MagazzinoRettifica,
                PermissionCatalog.VenditeRead
            },
            [Role.Commerciale] = new HashSet<string>
            {
                PermissionCatalog.AnagraficheRead,
                PermissionCatalog.AnagraficheWrite,
                PermissionCatalog.ArticoliRead,
                PermissionCatalog.VenditeRead,
                PermissionCatalog.VenditeWrite,
                PermissionCatalog.VenditeEmettiDocumento
            }
        };
}
