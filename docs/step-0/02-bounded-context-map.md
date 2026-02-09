# Bounded Context Map - Step 0

## 1. Contesti iniziali

## MasterData

Responsabilita:
- gestione anagrafiche clienti/fornitori/sedi/contatti
- gestione articoli e attributi base
- pubblicazione riferimenti validati agli altri contesti

Entita principali:
- Cliente
- Fornitore
- Sede
- Contatto
- Articolo
- CategoriaArticolo

## Inventory

Responsabilita:
- causali magazzino
- registrazione movimenti
- calcolo giacenze per sede/articolo

Entita principali:
- CausaleMagazzino
- MovimentoMagazzino
- Giacenza

## Sales

Responsabilita:
- gestione ciclo documenti vendita
- progressione stato documento
- emissione documenti finali

Entita principali:
- DocumentoVendita
- RigaDocumentoVendita
- StatoDocumentoVendita

## 2. Relazioni tra contesti

- `Sales` consuma referenze da `MasterData`:
  - ClienteId
  - ArticoloId
  - SedeId
- `Inventory` consuma referenze da `MasterData`:
  - ArticoloId
  - SedeId
- `Sales` produce eventi che influenzano `Inventory`:
  - conferma DDT/fattura genera movimenti coerenti.

## 3. Contratti inter-contesto (iniziali)

- Identificativi solo GUID.
- Date sempre UTC.
- Nessuna duplicazione non necessaria di dati anagrafici.
- Verifica esistenza referenze lato backend prima di persistere.

## 4. Glossario condiviso (estratto)

- `Sede`: unita organizzativa/logistica dell'azienda.
- `Giacenza`: saldo articolo per sede a un timestamp.
- `Documento vendita`: elemento del flusso preventivo -> ordine -> DDT -> fattura.
- `Rettifica`: variazione inventariale non derivata da flusso standard.
