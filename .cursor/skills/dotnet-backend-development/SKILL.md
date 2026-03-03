---
name: dotnet-backend-development
description: Provides .NET backend development guidance following C#, ASP.NET Core, and Entity Framework Core best practices. Use when working on C# backend code, ASP.NET Core APIs, or EF Core data access and the user expects idiomatic, production-grade .NET solutions.
---

# .NET Backend Development

## Instructions

Follow these rules whenever working on .NET backend code, especially C#, ASP.NET Core, and Entity Framework Core. Assume production-grade quality, testability, and maintainability are required.

### Code Style and Structure

- Write concise, idiomatic C# that follows official .NET and ASP.NET Core conventions.
- Organize code into layers such as Controllers, Models (domain/entities/DTOs), Services, Repositories (if used), and Infrastructure.
- Use object-oriented design (encapsulation, SOLID) and functional-style constructs (LINQ, immutability, pattern matching) where appropriate.
- Prefer LINQ and lambda expressions for collection operations when it improves clarity.
- Use descriptive, intention-revealing names like `IsUserSignedIn`, `CalculateTotal`, `CreateOrderAsync`.
- Keep methods focused and small; extract private helpers when a method becomes complex.
- Avoid duplication; factor reusable behavior into services, helpers, or extension methods.

### Naming Conventions

- Use **PascalCase** for class names, method names, properties, and public members.
- Use **camelCase** for local variables, parameters, and private fields (optionally prefixed with `_` if consistent with project style).
- Use **UPPERCASE** for constants (e.g., `DefaultPageSize` or `DEFAULT_PAGE_SIZE` depending on existing style).
- Prefix interface names with `I`, e.g., `IUserService`, `IOrderRepository`.
- Align with existing project naming patterns if they conflict with these defaults.

### C# and .NET Usage

- Use C# 10+ features when they make code clearer:
  - `record` / `record class` for immutable DTOs and value-like objects.
  - Pattern matching (`switch`, `is`, relational patterns) for branching.
  - Null-coalescing (`??`) and null-coalescing assignment (`??=`) for defaults.
- Leverage built-in ASP.NET Core features:
  - Dependency Injection, configuration, options pattern, logging, middleware, filters, model binding, and validation.
- Use Entity Framework Core effectively:
  - Use `DbContext` with `DbSet<TEntity>` for persistence.
  - Use async methods (`ToListAsync`, `FirstOrDefaultAsync`, etc.) for I/O-bound operations.
  - Track vs no-tracking queries appropriately (`AsNoTracking()` for read-only queries).
  - Configure relationships and constraints via Fluent API or Data Annotations as appropriate.

### Syntax and Formatting

- Follow the official C# Coding Conventions:
  - Consistent spacing, braces on new lines or same line as per project, using directives grouped and ordered, etc.
  - Use `var` when the type is obvious from the right-hand side; otherwise prefer explicit types.
- Use modern C# syntax:
  - Null-conditional operators (`?.`, `?[]`) to safely access nested members.
  - String interpolation (`$"User {userId}"`) instead of `string.Format`.
  - Expression-bodied members when they improve readability.
- Keep files focused: one main type per file when possible.

### Error Handling and Validation

- Use exceptions only for exceptional cases, not as a normal control-flow mechanism.
- Validate input at the boundaries:
  - Use Data Annotations or FluentValidation for request models.
  - Prefer model state validation in controllers (`ModelState.IsValid`) or filters.
- Implement global exception handling:
  - Use ASP.NET Core middleware (e.g., custom `ExceptionHandlingMiddleware`) or `UseExceptionHandler` for consistent error responses.
- Logging:
  - Use the built-in `ILogger<T>` abstraction (or a configured third-party provider) for logging.
  - Log enough context to diagnose issues without leaking sensitive data.
- HTTP error responses:
  - Return appropriate HTTP status codes (e.g., `400`, `401`, `403`, `404`, `409`, `500`).
  - Use consistent error response shapes (e.g., a standard error DTO with `code`, `message`, and `details`).

### API Design

- Follow RESTful API design principles:
  - Resource-oriented endpoints, proper use of HTTP verbs (GET, POST, PUT, PATCH, DELETE).
  - Use plural nouns for collections (e.g., `/api/users`, `/api/orders`).
- Use attribute routing on controllers and actions:
  - `[ApiController]`, `[Route("api/[controller]")]`, `[HttpGet]`, `[HttpPost]`, etc.
- Implement API versioning:
  - Use ASP.NET Core API Versioning or a similar approach (e.g., `api/v1/...`).
- Use filters for cross-cutting concerns:
  - Action filters or middleware for logging, authorization policies, validation, performance measurement, etc.

### Performance Optimization

- Use asynchronous programming with `async`/`await` for all I/O-bound operations (database, HTTP calls, file I/O).
- Avoid blocking calls (`.Result`, `.Wait()`) in asynchronous code paths.
- Implement caching where appropriate:
  - Use `IMemoryCache` for in-memory caching.
  - Use distributed cache (e.g., Redis) for multi-instance scenarios.
  - Define clear cache keys and expiration policies.
- EF Core performance:
  - Avoid N+1 query problems by including related data appropriately (`Include`, `ThenInclude`) or projecting into DTOs.
  - Prefer pagination for large data sets (e.g., `Skip`/`Take` with stable ordering).
  - Avoid unnecessary tracking where not needed (`AsNoTracking`).

### Key Conventions and Architecture

- Use Dependency Injection for loose coupling and testability:
  - Depend on abstractions (`IUserService`, `IEmailSender`) rather than concrete implementations.
  - Register services with appropriate lifetimes (`Singleton`, `Scoped`, `Transient`).
- Data Access:
  - For simple apps or microservices, using EF Core directly in services can be acceptable.
  - For more complex domains, consider repositories or domain-driven patterns where beneficial.
- Object Mapping:
  - Use AutoMapper or similar libraries when object-to-object mapping is non-trivial or repetitive.
  - Keep mapping profiles organized (e.g., per feature or per layer).
- Background work:
  - Use `IHostedService` or `BackgroundService` for background tasks.
  - Use queues or messaging systems (e.g., Azure Service Bus, RabbitMQ) for long-running or decoupled operations when appropriate.

### Testing

- Write automated tests:
  - Unit tests for domain logic and services (xUnit, NUnit, or MSTest).
  - Use mocking libraries such as Moq or NSubstitute for external dependencies.
  - Prefer constructor injection to simplify test setup.
- Integration and API tests:
  - Use the ASP.NET Core test host and `WebApplicationFactory` (or similar) to test endpoints.
  - Use an in-memory or test database configuration for EF Core.
- Aim for meaningful coverage:
  - Focus on critical paths, edge cases, and error handling.

### Security

- Use ASP.NET Core Authentication and Authorization middleware:
  - Configure authentication schemes, policies, and role-based or policy-based authorization.
- Implement JWT authentication for stateless APIs when appropriate:
  - Properly validate tokens, expiration, issuer, and audience.
- Always use HTTPS:
  - Enforce HSTS and redirect HTTP to HTTPS in production environments.
- Implement CORS policies:
  - Explicitly configure allowed origins, headers, and methods.
  - Avoid overly permissive CORS in production (`AllowAnyOrigin` only when truly justified).
- Protect sensitive data:
  - Never log secrets or sensitive user data.
  - Store configuration secrets securely (e.g., user secrets, environment variables, secret stores).

### API Documentation

- Use Swagger/OpenAPI for API documentation:
  - Configure Swashbuckle or a similar library.
  - Group endpoints logically, enable schema generation, and provide metadata (title, version, description).
- Add XML comments:
  - Use XML documentation comments on controllers, actions, and models to improve Swagger output.
- Keep documentation in sync with the implementation; update when changing routes, models, or behaviors.

## Examples

- When implementing a new ASP.NET Core controller, follow RESTful route design, use attribute routing, validate models using Data Annotations, and return appropriate HTTP status codes with consistent error shapes.
- When adding a new EF Core query, prefer async methods, avoid N+1 queries, and implement pagination for endpoints returning collections.
- When introducing a new service, define an interface (e.g., `IOrderService`), register it via DI, and cover its behavior with unit tests using mocks for infrastructure dependencies.

## When to Use This Skill

- Use this skill when:
  - Writing or reviewing C# backend code in ASP.NET Core.
  - Designing or refactoring APIs, controllers, services, or EF Core data access.
  - Implementing error handling, logging, security, or performance improvements in a .NET backend.
  - Adding tests around ASP.NET Core endpoints or business logic.

