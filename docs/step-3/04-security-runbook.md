# Step 3 - Security Runbook

## Configurazione richiesta
- `ConnectionStrings:SqlServer`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:SigningKey`
- `Jwt:ExpirationMinutes`
- `Jwt:RefreshTokenDays`

## Bootstrap ruoli/admin
- i ruoli Identity vengono seedati all'avvio.
- seed admin opzionale in Development:
  - `SecurityBootstrap:SeedAdmin=true`
  - `SecurityBootstrap:AdminUsername`
  - `SecurityBootstrap:AdminPassword`
  - `SecurityBootstrap:AdminCompanyId`

## Comandi utili
```powershell
dotnet ef database update --project src/backend/Erp.Infrastructure/Erp.Infrastructure.csproj --startup-project src/backend/Erp.Api/Erp.Api.csproj
dotnet run --project src/backend/Erp.Api/Erp.Api.csproj
dotnet test Erp.sln
```

## Incident response minimo
- monitorare login falliti e lockout da log strutturati.
- revocare refresh token tramite `/api/v1/auth/revoke`.
- verificare integrità claim (`sub`, `role`, `permission`, `company_id`) sui token emessi.
