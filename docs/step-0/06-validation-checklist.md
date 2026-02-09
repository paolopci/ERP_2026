# Step 0 - Validation Checklist

## 1. Completezza documentale

- [x] Visione prodotto e obiettivi business definiti.
- [x] In-scope e out-of-scope espliciti.
- [x] KPI di successo MVP definiti.
- [x] Backlog con epiche/feature/user story e AC.

## 2. Coerenza di perimetro

- [x] Ogni feature mappa a MasterData, Inventory o Sales.
- [x] Nessuna feature backlog contraddice out-of-scope.
- [x] Tenancy single-tenant dichiarata e coerente.

## 3. RBAC e sicurezza

- [x] Ruoli MVP fissi dichiarati.
- [x] Catalogo permessi definito.
- [x] Matrice ruolo-permesso definita.
- [x] Regola scenario deny per ogni ruolo definita.

## 4. NFR readiness

- [x] Performance target con metrica e soglia.
- [x] Audit policy con campi minimi.
- [x] GDPR baseline con retention iniziale.
- [x] Logging/traceability minimi definiti.

## 5. Contratti pubblici iniziali

- [x] `ApiEnvelope<T>`
- [x] `PagedRequest`
- [x] `PagedResult<T>`
- [x] `AuditRecord`
- [x] `Role` enum
- [x] Permission catalog iniziale

## 6. Exit criteria Step 0

Step 0 chiuso quando:
- tutti i deliverable documentali sono approvati dagli stakeholder
- i contratti cross-cutting sono accettati da backend/frontend
- la baseline NFR e il perimetro MVP risultano congelati per Step 1
