# Step 2 - Docker SQL Bootstrap

## Target runtime
- SQL Server in Docker: `localhost:1433`
- Database: `ErpDb`

## Prerequisiti
1. Container SQL avviato con port mapping `1433:1433`.
2. SDK .NET 9 disponibile.
3. Tool EF Core CLI disponibile (`dotnet-ef`).

## Configurazione credenziali (no secret in repo)
Impostare la connection string con user secrets nel progetto API:

```powershell
dotnet user-secrets --project src/backend/Erp.Api/Erp.Api.csproj set "ConnectionStrings:SqlServer" "Server=localhost,1433;Database=ErpDb;User Id=sa;Password=<YOUR_PASSWORD>;TrustServerCertificate=true;Encrypt=false"
```

Opzione alternativa via variabile ambiente:

```powershell
$env:ConnectionStrings__SqlServer="Server=localhost,1433;Database=ErpDb;User Id=sa;Password=<YOUR_PASSWORD>;TrustServerCertificate=true;Encrypt=false"
```

## Installazione dotnet-ef (se mancante)

```powershell
dotnet tool install --global dotnet-ef
```

## Update database
Eseguire le migration esistenti con startup API:

```powershell
dotnet ef database update --project src/backend/Erp.Infrastructure/Erp.Infrastructure.csproj --startup-project src/backend/Erp.Api/Erp.Api.csproj
```

## Smoke check readiness
Con API in esecuzione:

```powershell
Invoke-WebRequest http://localhost:5055/health/ready
```

Esito atteso: `200 OK` con connection string valida.
