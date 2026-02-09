# Step 1 - Operability

## Logging
- Serilog configurato da `appsettings`.
- Request logging abilitato.

## Telemetria
- OpenTelemetry con tracing HTTP e SQL client.
- Console exporter attivo in baseline.

## Health checks
- `/health/live`: check processo.
- `/health/ready`: check DbContext.

## Configuration
- `appsettings.json` + `appsettings.Development.json`.
- Binding opzioni con `IOptions` (`JwtOptions`).
- Supporto override via variabili ambiente.

## Deploy checklist minima
1. Connection string SQL valorizzata.
2. JWT signing key sicura in ambiente.
3. HTTPS e reverse proxy configurati.
4. Log/telemetry collection collegata a monitoraggio.
