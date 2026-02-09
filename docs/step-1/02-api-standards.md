# Step 1 - API Standards

## Versioning
- URL versioning obbligatorio: `/api/v1/...`.

## Response standard
- Successo: `ApiEnvelope<T>`.
- Errore: `application/problem+json` (ProblemDetails RFC7807) con `traceId`.

## Sicurezza
- Authentication: JWT bearer.
- Authorization: policy per permesso (`PermissionCatalog`).
- Default deny.

## Endpoint baseline
- `GET /health/live`
- `GET /health/ready`
- `POST /api/v1/masterdata/customers`
- `GET /api/v1/masterdata/customers/{id}`
- `GET /api/v1/inventory/summary`
- `GET /api/v1/sales/documents`

## Convenzioni
- `GUID` per identificativi.
- Timestamp in UTC.
- Controller versionati in route segment.
