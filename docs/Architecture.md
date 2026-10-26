# Tamam – Project Structure

## Layers (dependencies point inwards)

`Api -> Infrastructure -> Application -> Domain`

| Layer | Responsibility |
|---|---|
| **Domain** | Entities, enums, constants, domain exceptions. No dependency on other layers. |
| **Application** | Contracts (`Abstractions`), DTOs, service interfaces and business-logic services, validators, mappings. |
| **Infrastructure** | EF Core (`Persistence`), implementations that need external libraries (Identity/JWT, email, WhatsApp, storage). |
| **Api** | Controllers, filters, middleware, OpenAPI setup, services that need `HttpContext`. |

## Folder map

```
Tamam.Domain
  Common/            BaseEntity<TKey>, IAuditable, ISoftDeletable
  Constants/         RoleNames ...
  Entities/<Area>/   one class per file, grouped by business area
  Enums/             one enum per file
  Exceptions/        domain exceptions

Tamam.Application
  Abstractions/      IRepository, IUnitOfWork, IUserAppService, AppClaimTypes
  Common/            Result<T> and shared helpers
  DTOs/<Feature>/    request/response records, one type per file
  Services/<Feature>/ I<Feature>Service (+ implementation when it needs no external library)
  Settings/          strongly typed options (JwtSettings ...)
  Validators/        request validators
  Mappings/          entity <-> DTO mapping

Tamam.Infrastructure
  Persistence/Configurations/  IEntityTypeConfiguration<T>, one per entity
  Persistence/Repositories/    GenericRepository, UnitOfWork
  Persistence/Migrations/      EF migrations (generated)
  Persistence/Interceptors/    EF interceptors
  Persistence/Seed/            data seeding
  Services/<Feature>/          implementations that need external libraries
  Extensions/                  DI and helper extensions

Tamam.Api
  Controllers/       thin controllers, no business logic
  Filters/           MVC filters
  Middleware/        custom middleware
  OpenApi/           Swagger/OpenAPI transformers
  Services/          UserAppService (reads the JWT claims from HttpContext)
  Extensions/        Program.cs helpers
```

## Where does a new service go?

1. Interface + DTOs: `Application/Services/<Feature>` and `Application/DTOs/<Feature>`.
2. Implementation:
   - pure business logic over `IUnitOfWork` -> `Application/Services/<Feature>`
   - needs an external library (Identity, SMTP, WhatsApp API) -> `Infrastructure/Services/<Feature>`
   - needs `HttpContext` -> `Api/Services`
3. Register it in `DependencyInjection.cs` of the layer that owns the implementation.

## Request flow and responses

`Controller -> Service (Application) -> IUnitOfWork -> DbContext`

- Controllers inherit `ApiControllerBase`, never touch `IUnitOfWork`, and only call a service then `Respond(result)`.
- Every service returns `Result` / `Result<T>` (`Application/Common`). Factories: `Ok`, `Created`, `BadRequest`, `Unauthorized`, `Forbidden`, `NotFound`, `Conflict`, `UnprocessableEntity`, `InternalServerError`.
- Every HTTP response has the same JSON shape: `{ succeeded, statusCode, message, errors, data }`.
- Unhandled exceptions (`ExceptionHandlingMiddleware`), model-validation failures, and JWT 401/403 are returned in the same shape.
