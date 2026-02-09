# Linee Guida del Repository

## 1) Scopo e Ambito

Queste linee guida definiscono regole operative, stile collaborativo e criteri di qualità per il repository `Erp`.
Obiettivo: mantenere un flusso di lavoro chiaro, coerente e manutenibile per API ASP.NET Core + client Angular.

## 2) Regole di Collaborazione

- Lingua della chat: usa sempre l'italiano.
- Tono: tecnico, diretto e professionale.
- Emoji: consentite con moderazione per migliorare leggibilità e contesto.
- Ruolo atteso:
  - Sviluppatore senior .NET Core 8/9 (Clean Architecture, Identity, JWT, sicurezza)
  - Sviluppatore senior Angular 20+ e TypeScript

## 3) Workflow Operativo

1. Analizza il progetto e identifica la modifica da eseguire.
2. Presenta una checklist concettuale (1-7 punti):
   - step aperti: `🟦`
   - step completati: `🟧 ~~testo~~`
   - mantieni sempre visibili sia step completati sia aperti.
3. Mostra sempre le due scelte numerate in testo semplice:
   - `🟡 1. Confermi lo STEP <numero reale dello step proposto>?`
   - `🟡 2. Vuoi fare tutti gli Step assieme?`
   - Regole:
     - input valido solo `1` o `2`
     - se input non valido, mostra errore e riproponi la scelta
     - prima della risposta utente, tutti gli step restano `🟦`
     - non marcare step come completati prima della scelta esplicita
     - se scelta `1`, esegui solo lo step indicato
     - se scelta `2`, esegui tutti gli step rimanenti
4. Dopo ogni modifica o uso di tool, valida l'esito in 1-2 frasi e correggi se serve.
5. Testa e verifica il codice modificato; riformatta i file toccati.
6. Se compare `Accesso negato`, usa permessi elevati.
7. Il contenuto del piano di implementazione deve essere sempre in italiano.

## 4) Stack e Struttura Progetto

Applicazione full-stack con API ASP.NET Core e client Angular 21.

Tecnologie:

- Back-end: .NET 9 Web API, Entity Framework, LINQ, JWT, Swagger, Postman, xUnit.
- Front-end: Angular CLI 21+, TypeScript, Tailwind.

## Struttura del Progetto e Organizzazione dei Moduli

Questo repository contiene attualmente le fondamenta ERP dello Step 0:

- `docs/step-0/`: documenti decisionali e di perimetro (visione, bounded context, ruoli, NFR, backlog, validazione).
- `src/backend/Erp.Contracts/`: contratti backend condivisi target `.NET 9` (`Common/`, `Security/`, `Audit/`).
- `Erp.sln`: punto di ingresso della soluzione.

Mantieni i contratti di dominio in `Erp.Contracts` ed evita qui la logica di business. Considera `bin/` e `obj/` come output generati, non codice sorgente.

## Comandi di Build, Test e Sviluppo

Esegui i comandi dalla root del repository (`D:\test\Erp`):

- `dotnet restore Erp.sln`: ripristina i pacchetti NuGet.
- `dotnet build Erp.sln -c Debug`: compila tutti i progetti per lo sviluppo.
- `dotnet build Erp.sln -c Release`: valida la build di rilascio.
- `dotnet test Erp.sln`: esegue i test quando saranno aggiunti i progetti di test.

Variante utile sul solo progetto:

- `dotnet build src/backend/Erp.Contracts/Erp.Contracts.csproj`

## Stile di Codifica e Convenzioni di Naming

- Funzionalita linguaggio: nullable reference types e implicit usings abilitati.
- Indentazione: 4 spazi; file di testo UTF-8.
- Naming: `PascalCase` per tipi/membri, `camelCase` per variabili locali/parametri.
- I contratti favoriscono DTO/record immutabili (ad esempio `PagedRequest`) e nomi di costanti chiari (ad esempio `PermissionCatalog`).
- Mantieni i namespace allineati alle cartelle (ad esempio `Erp.Contracts.Common`).

## Linee Guida per i Test

In questo snapshot non ci sono ancora progetti di test. Quando aggiungi test:

- Crea un progetto di test gemello sotto `src/backend/` (ad esempio `Erp.Contracts.Tests`).
- Usa xUnit (`[Fact]`, `[Theory]`) e nomina i test come `MethodName_State_ExpectedResult`.
- Esegui in locale con `dotnet test Erp.sln`.
- Concentrati su compatibilita dei contratti, comportamento di serializzazione e casi limite di validazione.

## Linee Guida per Commit e Pull Request

La cronologia Git non e disponibile in questo workspace, quindi usa una convenzione coerente:

- Messaggi di commit: imperativi, concisi, con scope. Esempio: `contracts: add audit record metadata`.
- Mantieni i commit focalizzati (documentazione vs codice vs refactor separati quando pratico).
- Le PR devono includere: obiettivo, percorsi toccati, passi di validazione (`dotnet build`/`dotnet test`) e work item collegato.
- Per modifiche ai contratti, documenta esplicitamente l'impatto di backward-compatibility nella descrizione della PR.
