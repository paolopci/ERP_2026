# Step 3 - Tenant `CompanyId`

## Decisione
Runtime MVP single-company, ma `CompanyId` obbligatoria nel modello auth e nelle entità business principali.

## Applicazione tecnica
- claim obbligatoria: `company_id`.
- policy default authorization: richiede `company_id`.
- entità con `CompanyId`:
  - `Cliente`
  - `Articolo`
  - `MovimentoMagazzino`
  - `DocumentoVendita`
- filtri repository: query customer isolate per `CompanyId` corrente.

## Contratto operativo
- token senza `company_id` -> accesso endpoint `[Authorize]` negato.
- accesso a dati di altra azienda non consentito.
