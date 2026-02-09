# ERP - Step 0, Step 1, Step 2 Foundation

Repository bootstrap per gli step iniziali del progetto ERP con:

- documentazione decisionale completa (`docs/step-0`)
- baseline architetturale Step 1 (`docs/step-1`, backend modular monolith, frontend Angular)
- baseline persistenza/configurazione Step 2 (`docs/step-2`)
- contratti pubblici backend iniziali (`src/backend/Erp.Contracts`) target `.NET 9`

## Struttura

- `docs/step-0/01-product-vision-scope.md`
- `docs/step-0/02-bounded-context-map.md`
- `docs/step-0/03-role-permission-matrix.md`
- `docs/step-0/04-nfr-baseline.md`
- `docs/step-0/05-mvp-backlog.md`
- `docs/step-0/06-validation-checklist.md`
- `docs/step-1/*`
- `docs/step-2/*`
- `src/backend/Erp.Api/*`
- `src/backend/Erp.Application/*`
- `src/backend/Erp.Domain/*`
- `src/backend/Erp.Infrastructure/*`
- `src/frontend/erp-web/*`
- `src/backend/Erp.Contracts/*`

## Scope

- Step 0: visione, perimetro MVP, ruoli, NFR, backlog e contratti cross-cutting.
- Step 1: setup soluzione Clean Architecture / Modular Monolith con baseline API, DI, logging, config, health checks, quality gate e test iniziali.
- Step 2 addendum: allineamento SQL Server Docker con credenziali via user-secrets/env vars.
