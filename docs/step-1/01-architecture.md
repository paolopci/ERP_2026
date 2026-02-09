# Step 1 - Architecture

## Obiettivo
Definire una baseline Modular Monolith con Clean Architecture per i moduli ERP.

## Struttura
- `Erp.Api`: presentation layer.
- `Erp.Application`: use case CQRS e pipeline behavior.
- `Erp.Domain`: entita, value object, domain event.
- `Erp.Infrastructure`: EF Core, repository, integrazioni tecniche.
- `Erp.Contracts`: contratti API condivisi.

## Regole dipendenze
1. `Api -> Application, Infrastructure, Contracts`
2. `Application -> Domain, Contracts`
3. `Infrastructure -> Application, Domain`
4. `Domain -> nessuna dipendenza`

## Moduli dominio
- `MasterData`
- `Inventory`
- `Sales`

## Scelte tecniche
- CQRS leggero con MediatR.
- DbContext unico (`ErpDbContext`) con separazione schema logica.
- JWT + policy RBAC basate su permessi.
