# Product Vision & Scope - Step 0

## 1. Visione prodotto

Realizzare un ERP moderno, modulare e cloud-ready per PMI, focalizzato su velocita operativa e tracciabilita dei processi core commerciali e logistici.

## 2. Obiettivi business MVP

- Ridurre il tempo di gestione ordini e documenti vendita.
- Migliorare accuratezza giacenze per sede.
- Uniformare anagrafiche clienti/fornitori/articoli.
- Garantire controllo accessi basato su ruolo.
- Abilitare audit completo CRUD su entita critiche.

## 3. Target utenti

- `Admin`: configurazione e governo piattaforma.
- `Contabile`: controllo documenti vendita e consultazione dati rilevanti.
- `Magazziniere`: operazioni su movimenti e giacenze.
- `Commerciale`: gestione flusso documentale vendita.

## 4. Scope funzionale MVP

### In scope

1. Anagrafiche
- Clienti, fornitori, sedi, contatti principali.
- Stato attivo/inattivo.

2. Articoli
- SKU, descrizione, unita di misura, categoria.
- Prezzo base e stato.

3. Magazzino base
- Causali di magazzino.
- Movimenti entrata/uscita/rettifica.
- Giacenza per sede.

4. Documenti vendita
- Preventivo, ordine, DDT, fattura.
- Catena minima configurabile nel backlog.

### Out of scope

- Ciclo passivo acquisti completo.
- Contabilita generale avanzata.
- Multi-lingua UI.
- Ruoli custom dinamici.
- Multi-tenant operativo.

## 5. Vincoli architetturali confermati

- Backend: `.NET 9`.
- Frontend: `Angular 21`.
- Database: `SQL Server` (unico per MVP).
- Modello tenancy: `single-tenant` (1 azienda).
- Multi-sede: supportata.
- Deploy: cloud-first containerizzato (`dev/test/prod`).

## 6. KPI di successo MVP

- Adozione: almeno 80% processi vendita core gestiti nel sistema in pilot.
- Qualita dati: mismatch giacenze ridotto del 50% rispetto baseline.
- Efficienza: riduzione tempo emissione documento finale >= 30%.
- Affidabilita API: P95 CRUD < 500ms su dataset medio.
- Sicurezza: 0 endpoint core esposti senza controllo RBAC.

## 7. Stakeholder e governance

- Product Owner: priorita business e accettazione scope.
- Tech Lead: coerenza architetturale e contratti API.
- Referente Operations: validazione flussi magazzino.
- Referente Finance: validazione flussi documenti vendita.

## 8. Rischi principali e mitigazioni

- Rischio creep scope: blocco out-of-scope approvato in steering.
- Rischio ambiguita ruoli: matrice permessi formale + scenari deny.
- Rischio performance tardiva: target NFR formalizzati nello Step 0.
- Rischio compliance incompleta: baseline GDPR e audit stabilite ora.
