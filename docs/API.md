# Tamam API – Endpoints for the frontend

Base URL: `https://localhost:7265` (dev). Swagger UI: `/swagger`.

Every response uses the same envelope:

```json
{ "succeeded": true, "statusCode": 200, "message": null, "errors": [], "data": { } }
```

Failures use the same shape with `succeeded: false`, an HTTP status (400, 401, 403, 404, 409, 410, 500) and `errors`/`message`.
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
Frontend routes expected: `{PublicLinks:BaseUrl}/c/{token}` (confirmation) and `/s/{token}` (rejection survey).

## Others
`GET /api/messages/templates` · `GET /api/billing/plans|subscription|usage` · `GET /api/notifications`, `/unread-count`, `POST /{id}/read`, `/read-all` · `GET /api/reports/dashboard?from=&to=` · `GET /api/reports/quality?months=6`

## Required configuration (`appsettings.Development.json`)
See `src/Tamam.Api/appsettings.Example.json`: `ConnectionStrings`, `Jwt`, `PublicLinks`.
