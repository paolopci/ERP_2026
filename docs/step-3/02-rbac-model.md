# Step 3 - Modello RBAC

## Ruoli MVP
- `Admin`
- `Contabile`
- `Magazziniere`
- `Commerciale`

## Principio
- ruoli persistiti in tabelle Identity (`identity.Roles`, `identity.UserRoles`);
- permessi non dinamici: derivati da `RolePermissions` (`Erp.Contracts`).

## Claims emesse
- `sub`: id utente
- `role`: ruoli assegnati
- `permission`: permessi risolti dai ruoli
- `company_id`: contesto aziendale corrente

## Enforcement
- endpoint business protetti con `[Authorize(Policy = PermissionCatalog.*)]`.
- assenza permesso -> `403 Forbidden`.
- assenza token -> `401 Unauthorized`.
