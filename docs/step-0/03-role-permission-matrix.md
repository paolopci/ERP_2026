# Role & Permission Matrix - Step 0

## 1. Ruoli MVP

- `Admin`
- `Contabile`
- `Magazziniere`
- `Commerciale`

## 2. Catalogo permessi

- `anagrafiche.read`
- `anagrafiche.write`
- `articoli.read`
- `articoli.write`
- `magazzino.read`
- `magazzino.movimenta`
- `magazzino.rettifica` (sensibile)
- `vendite.read`
- `vendite.write`
- `vendite.emetti-documento` (sensibile)
- `audit.read`
- `administration.full`

## 3. Matrice ruolo -> permesso

| Permesso | Admin | Contabile | Magazziniere | Commerciale |
|---|---|---|---|---|
| anagrafiche.read | Y | Y | Y | Y |
| anagrafiche.write | Y | N | N | Y |
| articoli.read | Y | Y | Y | Y |
| articoli.write | Y | N | N | N |
| magazzino.read | Y | N | Y | N |
| magazzino.movimenta | Y | N | Y | N |
| magazzino.rettifica | Y | N | Y | N |
| vendite.read | Y | Y | Y | Y |
| vendite.write | Y | Y | N | Y |
| vendite.emetti-documento | Y | Y | N | Y |
| audit.read | Y | Y | N | N |
| administration.full | Y | N | N | N |

## 4. Azioni sensibili

- Emissione documento vendita: richiede `vendite.emetti-documento`.
- Rettifica giacenza: richiede `magazzino.rettifica`.
- Consultazione audit completo: richiede `audit.read`.
- Operazioni amministrative: richiede `administration.full`.

## 5. Regole enforcement

- Backend: autorizzazione obbligatoria su ogni endpoint.
- Frontend: visibilita azioni guidata da permessi, ma enforcement reale lato API.
- Ogni ruolo deve avere almeno uno scenario deny testato.
