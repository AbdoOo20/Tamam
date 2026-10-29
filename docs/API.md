# Tamam API – Endpoints for the frontend

Base URL: `https://localhost:7265` (dev). Swagger UI: `/swagger`.

Every response uses the same envelope:

```json
{ "succeeded": true, "statusCode": 200, "message": null, "errors": [], "data": { } }
```

Failures use the same shape with `succeeded: false`, an HTTP status (400, 401, 403, 404, 409, 410, 429, 500) and `errors`/`message`.
Authenticated endpoints need `Authorization: Bearer <accessToken>`.
Paged endpoints return `data = { items, page, pageSize, totalCount, totalPages, hasNext, hasPrevious }` and accept `?page=1&pageSize=20&search=`.
Enums are numbers; use `GET /api/lookups/enums` for names/values.

## Auth (anonymous unless noted)
| Method | Route | Notes |
|---|---|---|
| POST | `/api/auth/register` | creates the merchant + Owner + 14-day trial subscription, returns a JWT |
| POST | `/api/auth/login` | returns a JWT (`merchant_id`, `role` claims) |
| GET | `/api/auth/me` | current user from the token |

## Staff (Owner for write)
`GET /api/staff` · `POST /api/staff` · `DELETE /api/staff/{id}` (soft delete)

## Merchant
`GET/PUT /api/merchant/profile` (PUT: Owner) · `GET/POST /api/merchant/consent` (POST: Owner, controls network sharing)

API key for store integrations (Owner): `GET /api/merchant/api-key` (`{ hasKey }`) · `POST /api/merchant/api-key` (generates/rotates, the key is returned **once**) · `DELETE /api/merchant/api-key` (revoke)

## Lookups
`GET /api/countries` (anonymous) · `GET /api/countries/{code}` · `GET /api/lookups/regions?countryCode=EG` · `order-statuses` · `rejection-reasons` · `risk-rules` · `enums`

## Carriers / Products / Customers
`/api/carriers` CRUD · `/api/products` CRUD (+ `?isActive=`) · `GET /api/customers`, `GET/PUT /api/customers/{id}`

## Orders
| Method | Route | Notes |
|---|---|---|
| GET | `/api/orders` | filters: `statusId, riskLevel, carrierId, from, to, search` |
| POST | `/api/orders` | create: customer upsert, risk score + explanations, confirmation link + wa.me message |
| GET/PUT/DELETE | `/api/orders/{id}` | detail (items, history, attempts, risk, confirmation, outcome) |
| PUT | `/api/orders/{id}/items` | replace the order items (only while New / AwaitingConfirmation / Confirmed / DepositRequested); totals are recalculated |
| POST | `/api/orders/{id}/status` | `{ statusId, note }`, validated transitions |
| POST | `/api/orders/{id}/delivery-attempts` | may move the order to Delivered/Refused/FailedDelivery |
| POST | `/api/orders/import` | bulk create (max 200 rows), historical or regular |
| GET | `/api/orders/{id}/confirmation` | link, message body, `whatsAppUrl` |
| POST | `/api/orders/{id}/confirmation/sent` | merchant pressed "send" (marks Sent, status -> AwaitingConfirmation) |
| POST | `/api/orders/{id}/confirmation/regenerate` | new link/attempt |
| GET | `/api/orders/{id}/messages` | message log |
| GET | `/api/orders/{id}/risk` · POST `/risk/recalculate` · POST `/risk/decision` | risk history and merchant decision |
| GET | `/api/orders/{id}/rejections` · `/outcome` · PUT `/outcome/loss` | rejection reports, attribution, loss |

## Rejections
`POST /api/rejections` – merchant reports the refusal reason (order must be Refused/Returned). Attribution (Customer / Merchant / Carrier / Neutral) is resolved from the reason and the customer's own answer; conflicting answers become a dispute.

## Public pages (anonymous, token from the link)
`GET /api/public/links/{token}` · `POST .../confirm` · `.../reschedule` · `.../cancel` · `.../survey` · `.../opt-out` · `GET /api/public/rejection-reasons`
`POST /api/public/links/{token}/data-request` with `{ type }` (2 = Delete, 3 = Access) lets the customer ask for deletion/access of their data; an admin reviews it.
Frontend routes expected: `{PublicLinks:BaseUrl}/c/{token}` (confirmation) and `/s/{token}` (rejection survey).

## Billing
`GET /api/billing/plans` · `GET /api/billing/subscription` · `GET /api/billing/usage?from=&to=` · `POST /api/billing/subscription/change` with `{ planCode }` (Owner). There is no online payment yet: changing the plan activates it immediately and billing is handled manually from the usage report.

## Store integration (API key, no JWT)
Send `X-Api-Key: tmk_...` (generated from `POST /api/merchant/api-key`).

| Method | Route | Notes |
|---|---|---|
| POST | `/api/integrations/orders` | same body as `POST /api/orders`; idempotent on `orderNo` (a repeated `orderNo` returns the existing order with `alreadyExisted: true`). Returns the risk score/level, recommended action, confirmation link and wa.me URL |
| GET | `/api/integrations/orders/{orderNo}` | current status and risk of an order |

## Audit log (Owner)
`GET /api/auditlogs?entityName=&entityId=&action=&userId=&from=&to=&page=` – who changed what (orders, products, carriers, merchant, users, subscriptions...). `oldValues` / `newValues` are JSON strings with only the changed fields.

## Platform admin (role `PlatformAdmin`)
The first admin is created at startup from the `PlatformAdmin` configuration section. Log in with `POST /api/auth/login`; the token has no `merchant_id`.

| Method | Route | Notes |
|---|---|---|
| GET | `/api/admin/overview` | merchants, orders, 30-day refusal rate, open data requests |
| GET | `/api/admin/merchants?status=&search=` | paged list |
| GET | `/api/admin/merchants/{id}` | details, plan, counts |
| PUT | `/api/admin/merchants/{id}/status` | `{ status }` Active / Suspended / Closed (suspended merchants cannot use the API key) |
| PUT | `/api/admin/merchants/{id}/subscription` | `{ planCode }` |
| GET/POST/PUT | `/api/admin/plans` · `/api/admin/plans/{id}` | plans (code cannot change) |
| GET/PUT | `/api/admin/risk-rules` · `/api/admin/risk-rules/{id}` | edit points, texts, active flag |
| GET | `/api/admin/data-requests?status=&type=` | customer data requests |
| POST | `/api/admin/data-requests/{id}/process` | `{ approve }`; Delete anonymizes the customer, Access is exported with the next endpoint |
| GET | `/api/admin/data-requests/{id}/export` | customer data for an Access request |
| GET | `/api/admin/audit-logs?merchantId=` | audit log across merchants |
| POST | `/api/admin/quality/recalculate` | `{ year?, month? }` rebuild the monthly quality snapshots |

## Real-time notifications (SignalR)

- Hub: `/hubs/notifications` (needs a JWT; browsers pass it as `?access_token=<jwt>` because WebSockets cannot send headers).
- On connect the user joins `merchant:{merchantId}` and `user:{userId}`.
- Event `notification` carries a `NotificationDto` (`id, type, title, body, relatedOrderId, isRead, createdAt`). It is sent right after a `Notification` row is saved: to the user group when `UserId` is set, otherwise to the whole merchant group.
- `GET /api/notifications` and `/unread-count` stay the source of truth; the client re-reads them after a reconnect.
- CORS must list the frontend origin in `Cors:AllowedOrigins` (the policy allows credentials).

## Others
`GET /api/messages/templates` · `GET /api/notifications`, `/unread-count`, `POST /{id}/read`, `/read-all` · `GET /api/reports/dashboard?from=&to=` · `GET /api/reports/quality?months=6`

## Rate limits (per IP, per minute)
Public links 120 · login/register 20 · store integration 300. Exceeding returns `429` with a `Retry-After` header.

## Background job
Every hour: refreshes the monthly quality snapshots, marks expired confirmation requests as `NoResponse`, and notifies merchants whose trial ends within 3 days.

## Required configuration (`appsettings.Development.json`)
See `src/Tamam.Api/appsettings.Example.json`: `ConnectionStrings`, `Jwt`, `PublicLinks`, and optional `PlatformAdmin` (`Email`, `Password`, `FullName`).
