# Northwind.App.Backend - AI Instructions

## Project Overview

This is an ASP.NET Core REST API backend application serving as a demo/reference implementation for best practices.

## Technology Stack

- **.NET 10** (or latest)
- **ASP.NET Core Web API**
- **Serilog** for structured logging
- **Swashbuckle** for OpenAPI/Swagger documentation

## Architecture & Best Practices

### Implemented Patterns

1. **Structured Logging** - Serilog with console sink (container-ready)
2. **Health Checks** - Liveness (`/health/live`) and Readiness (`/health/ready`) endpoints
3. **Problem Details (RFC 7807)** - Consistent error response format
4. **Response Compression** - Gzip/Brotli enabled for HTTPS
5. **CORS** - Configured to allow all origins (demo purposes)
6. **OpenAPI/Swagger** - Full API documentation with annotations

### Code Guidelines

- **Language**: All code, comments, logs, and documentation must be in **English**
- **Logging**: Use `ILogger<T>` (backed by Serilog), never `Console.WriteLine`
- **Controllers**: Use proper `[ProducesResponseType]` attributes for Swagger
- **Async**: Use async/await for I/O operations
- **Error Handling**: Let exceptions bubble up - global handler returns Problem Details

### Project Structure

```
├── Controllers/          # API controllers
│   └── SystemController  # Health, version, config endpoints
├── Program.cs            # Application entry point & configuration
├── appsettings.json      # Serilog and app configuration
└── .github/              # GitHub and AI instructions
```

### API Endpoints

| Endpoint | Purpose |
|----------|---------|
| `/health` | Basic health check (built-in) |
| `/health/live` | Liveness probe for Kubernetes |
| `/health/ready` | Readiness probe with dependency checks |
| `/version` | Application version |
| `/config` | Runtime configuration info |
| `/test` | Echo endpoint for testing |
| `/test/error` | Throws exception to demo Problem Details |
| `/swagger` | OpenAPI documentation UI |

### Container Considerations

- Logs go to stdout (console) - collected by container runtime
- Health endpoints compatible with Kubernetes probes
- No file system dependencies
- Culture set to en-US for consistent number/date formatting

### Not Yet Implemented (Intentionally)

- **Authentication/Authorization** - Add when needed
- **Database** - Add EF Core or Dapper as needed
- **Caching** - Add Redis/MemoryCache as needed
- **Rate Limiting** - Consider for production

## When Modifying This Project

1. Keep all text in English
2. Add `[ProducesResponseType]` to new endpoints
3. Use structured logging: `_logger.LogInformation("Message {Parameter}", value)`
4. Add XML documentation comments to public API methods
5. Follow existing patterns in SystemController
