# NFR Baseline - Step 0

## 1. Performance

- Target: `P95 < 500ms` per endpoint CRUD principali su dataset medio.
- Misura: percentile per endpoint con carico realistico su ambiente test.
- Ambito iniziale: anagrafiche, articoli, movimenti magazzino, documenti vendita.

## 2. Sicurezza

- Autenticazione centralizzata (JWT/OIDC decisione tecnica finale nello Step 1).
- RBAC rigido basato su ruolo + permessi.
- Negazione per default: assenza permesso = accesso negato.
- Correlation ID per tracciamento richieste sensibili.

## 3. Audit

- Audit CRUD completo su:
  - Cliente
  - Articolo
  - MovimentoMagazzino
  - DocumentoVendita
- Campi minimi audit:
  - entity
  - entityId
  - action
  - userId
  - timestampUtc
  - oldValues
  - newValues

## 4. GDPR baseline

- Minimizzazione dati personali nei modelli.
- Tracciabilita accessi amministrativi.
- Bozza retention policy:
  - audit: 24 mesi
  - log applicativi: 90 giorni
  - dati operativi ERP: secondo obblighi di legge/contrattuali

## 5. Disponibilita e operativita

- Ambienti: `dev`, `test`, `prod`.
- Deploy cloud-first containerizzato.
- Recovery: backup DB pianificato (RPO/RTO dettagliati nello Step 1).

## 6. Osservabilita minima

- Logging strutturato JSON.
- Correlation/trace ID in ogni risposta API.
- Dashboard:
  - errore rate
  - latenza P95
  - throughput endpoint principali
