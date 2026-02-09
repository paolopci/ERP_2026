# Step 3 - Architettura Auth

## Obiettivo
Introdurre autenticazione reale con `ASP.NET Identity + JWT`, mantenendo il contratto claims stabile per evoluzioni future (OpenIddict/Entra ID).

## Componenti
- `IIdentityService`: validazione credenziali e profilo utente corrente.
- `ITokenIssuer`: emissione access token, refresh token rotation e revoke.
- `ITenantContext`: lettura `company_id` da claim.
- `AuthController`: endpoint `/api/v1/auth/login|refresh|revoke|me`.

## Flusso Login
1. `POST /api/v1/auth/login`
2. `IdentityService` verifica password/lockout.
3. `JwtTokenIssuer` emette:
   - access token (breve durata)
   - refresh token persistito come hash
4. risposta `ApiEnvelope<TokenResponse>`.

## Flusso Refresh
1. client invia refresh token.
2. lookup hash su `identity.RefreshTokens`.
3. token attivo: revoca token precedente + crea nuovo token.
4. restituisce nuova coppia access/refresh.

## Sicurezza minima
- `MapInboundClaims=false` per preservare claim contract.
- policy di default: utente autenticato + claim `company_id`.
- policy per permesso: claim `permission`.
