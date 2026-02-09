# MVP Backlog - Step 0

## 1. Priorita

- `P0`: indispensabile al go-live MVP
- `P1`: importante ma differibile dopo stabilizzazione

## 2. Epiche, feature e user story

## EPIC E1 - Foundation & Security (P0)

### F1.1 Contratti API cross-cutting (M)
US1:
- Come team backend voglio uno standard uniforme di risposta API per garantire coerenza client.
- AC:
  - esiste `ApiEnvelope<T>` con `data/errors/meta/traceId`
  - errori standardizzati con `code/message/target`

US2:
- Come team backend voglio contratti paging riusabili.
- AC:
  - esistono `PagedRequest` e `PagedResult<T>`
  - supporto sort e filtri tipizzati

### F1.2 RBAC base (L)
US1:
- Come Admin voglio assegnare ruoli fissi MVP.
- AC:
  - ruoli disponibili: Admin, Contabile, Magazziniere, Commerciale
  - enforcement su API core

US2:
- Come responsabile sicurezza voglio scenari deny testati.
- AC:
  - almeno uno scenario deny per ruolo

## EPIC E2 - MasterData (P0)

### F2.1 Anagrafiche clienti/fornitori/sedi/contatti (L)
US1:
- CRUD Cliente con stato attivo/inattivo.
- AC:
  - create/read/update/delete disponibili
  - validazioni obbligatorie su campi principali

US2:
- CRUD Fornitore e gestione contatti.
- AC:
  - contatto principale definibile

US3:
- CRUD Sede.
- AC:
  - sede associabile a movimenti e documenti

### F2.2 Articoli (M)
US1:
- CRUD Articolo con SKU univoco.
- AC:
  - SKU univoco per azienda
  - UM e categoria obbligatorie

## EPIC E3 - Inventory (P0)

### F3.1 Causali e movimenti magazzino (L)
US1:
- Registrazione entrata/uscita.
- AC:
  - movimenti persistiti con timestamp UTC
  - blocco se articolo/sede non validi

US2:
- Rettifica giacenza controllata.
- AC:
  - permesso specifico richiesto
  - audit obbligatorio

### F3.2 Giacenza per sede (M)
US1:
- Consultazione saldo articolo per sede.
- AC:
  - query paginata e filtrabile

## EPIC E4 - Sales Documents (P0)

### F4.1 Flusso documenti vendita minimo (L)
US1:
- Creazione preventivo/ordine/DDT/fattura.
- AC:
  - stati documento tracciati
  - progressione valida tra stati

US2:
- Emissione documento finale.
- AC:
  - permesso `vendite.emetti-documento` obbligatorio
  - audit evento emissione

## EPIC E5 - Audit & Observability (P0)

### F5.1 Audit CRUD (M)
US1:
- Tracciamento old/new values entita critiche.
- AC:
  - `AuditRecord` registrato su create/update/delete

### F5.2 Logging e metriche base (M)
US1:
- Tracciamento latenza ed errori endpoint principali.
- AC:
  - dashboard con P95 e error rate disponibile

## 3. Dipendenze principali

- E2 dipende da E1 (contratti + RBAC).
- E3 dipende da E2 (Articoli e Sedi).
- E4 dipende da E2 (Clienti/Articoli) e parzialmente E3.
- E5 trasversale, attivata da subito e completata con E2-E4.

## 4. Sequenza consigliata implementazione

1. E1 Foundation & Security
2. E2 MasterData
3. E3 Inventory
4. E4 Sales Documents
5. E5 hardening finale e validazione NFR
