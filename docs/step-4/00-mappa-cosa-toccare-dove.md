# Step 4 — Mappa sintetica “cosa toccare dove”

Obiettivo: fornire una guida rapida (1 pagina) per implementare una feature end-to-end nel progetto ERP, indicando i punti di modifica su API, Angular e test.

## 1) API — Aggiungere endpoint `GET /api/v1/customers/{id}`

**Contratti (se cambia la response):**
- `src/backend/Erp.Contracts/Common/*` (solo se servono envelope/meta aggiuntivi)
- `src/backend/Erp.Contracts` (DTO condivisi; evitare logica di business)

**Application layer (use case):**
- `src/backend/Erp.Application/MasterData/Customers/Queries/GetCustomerById/GetCustomerByIdQuery.cs`
- `src/backend/Erp.Application/MasterData/Customers/Queries/GetCustomerById/GetCustomerByIdQueryHandler.cs`
- Eventuale validazione: validator nella stessa cartella del caso d’uso

**Infrastructure (accesso dati):**
- `src/backend/Erp.Infrastructure/Persistence/Repositories/CustomerRepository.cs`
- `src/backend/Erp.Application/Abstractions/Persistence/ICustomerRepository.cs` (allineare firma)

**API layer (controller):**
- `src/backend/Erp.Api/Controllers/MasterDataController.cs`
- Aggiungere route versionata e mapping request/response coerente con standard API

---

## 2) Angular — Aggiungere pagina `Customer Detail`

**Routing e shell:**
- `src/frontend/erp-web/src/app/app.routes.ts` (nuova route)
- `src/frontend/erp-web/src/app/app.html` (link/menu se previsto)

**Client API:**
- `src/frontend/erp-web/src/app/core/api/api.service.ts`
- Aggiungere metodo tipizzato per chiamare `GET /api/v1/customers/{id}`

**Feature UI:**
- Nuova cartella consigliata: `src/frontend/erp-web/src/app/features/customers/customer-detail/`
  - `customer-detail.component.ts`
  - `customer-detail.component.html`
  - `customer-detail.component.scss`

**Cross-cutting (se necessario):**
- `src/frontend/erp-web/src/app/core/http/trace-id.interceptor.ts` (solo se cambiano header/tracing)

---

## 3) Test — Aggiungere copertura minima utile

**Application tests (xUnit):**
- `tests/backend/Erp.Application.Tests/MasterData/CreateCustomerCommandTests.cs`
- Aggiungere test query handler (nuovo file consigliato):
  - `tests/backend/Erp.Application.Tests/MasterData/GetCustomerByIdQueryTests.cs`

**API smoke/integration:**
- `tests/backend/Erp.Api.SmokeTests/ApiSmokeTests.cs`
- Verificare status code, payload base, e casi not-found

**Naming consigliato test:**
- `MethodName_State_ExpectedResult`

---

## 4) Checklist operativa veloce

1. Aggiorna contratti/DTO solo se necessario (valuta backward compatibility).
2. Implementa query/handler + repository.
3. Esponi endpoint nel controller API.
4. Collega Angular route + servizio + componente pagina.
5. Esegui test backend e build full solution.

Comandi di verifica:
- `dotnet restore Erp.sln`
- `dotnet build Erp.sln -c Debug`
- `dotnet test Erp.sln`

Nota BC: per modifiche ai contratti in `Erp.Contracts`, documentare sempre impatto backward-compatibility nella PR.
