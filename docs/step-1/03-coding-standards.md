# Step 1 - Coding Standards

## Convenzioni
- C# con nullable enabled.
- Naming: PascalCase (tipi/membri), camelCase (variabili locali).
- Namespace coerenti con struttura cartelle.

## Qualita
- `Directory.Build.props` con analyzers e warning-as-error sui progetti core.
- `.editorconfig` condiviso per stile e formattazione.

## Pattern
- MediatR per command/query.
- FluentValidation per input validation.
- Behavior pipeline per cross-cutting concern.

## Test
- xUnit per unit e smoke integration.
- Almeno un test happy path e uno failure path per use case.
