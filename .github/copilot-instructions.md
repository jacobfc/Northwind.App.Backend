# Northwind.App.Backend - AI Instructions

## Project Overview

This is an ASP.NET Core REST API backend application serving as a demo/reference implementation for best practices. It provides a complete RESTful API for the Northwind database with both public and authenticated endpoints.

## Technology Stack

- **.NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core** with SQLite database
- **JWT Bearer Authentication** for secure endpoints
- **Serilog** for structured logging
- **DotNetEnv** - .env file support for environment variables
- **Swashbuckle** for OpenAPI/Swagger documentation
- **Meziantou.Analyzer** - Code quality analyzer enforcing best practices

## Architecture & Best Practices

### Implemented Patterns

1. **JWT Authentication** - Token-based authentication with access and refresh tokens
2. **Entity Framework Core** - Code-first approach with SQLite database
3. **Structured Logging** - Serilog with console sink and smart request filtering
4. **Environment Variables** - DotNetEnv for .env file support in development
5. **Version Management** - Dynamic app name and version from assembly metadata
6. **Health Checks** - Liveness and Readiness endpoints for container orchestration
7. **Problem Details (RFC 7807)** - Consistent error response format
8. **Response Compression** - Gzip/Brotli enabled for HTTPS
9. **CORS** - Configured to allow all origins (demo purposes)
10. **OpenAPI/Swagger** - Full API documentation with annotations and JWT authentication support
11. **Docker Support** - Multi-stage Dockerfile with HEALTHCHECK directive
12. **Cloud Deployment** - render.yaml configuration for Render.com deployment
13. **Docker Compose** - Local development orchestration with .env file support

### Code Guidelines

- **Language**: All code, comments, logs, and documentation must be in **English**
- **Logging**: Use `ILogger<T>` (backed by Serilog), never `Console.WriteLine`
- **Controllers**: Use proper `[ProducesResponseType]` attributes for Swagger
- **Async**: Use async/await for I/O operations
- **Error Handling**: Let exceptions bubble up - global handler returns Problem Details
- **Authentication**: Use `[Authorize]` attribute for protected endpoints
- **Database**: Use `AsNoTracking()` for read-only queries
- **Code Quality**: Build must compile with **zero warnings** - Dockerfile uses `--warnaserror` to enforce this
- **Analyzers**: Meziantou.Analyzer is enabled with simplified warnings disabled (see `.editorconfig`)

### Project Structure

```
├── Controllers/                     # API controllers
│   ├── SystemController.cs          # Health, version, config endpoints
│   ├── AuthController.cs            # JWT authentication (login, refresh, logout)
│   ├── CustomersController.cs       # Protected customer endpoints
│   └── PublicCustomersController.cs # Public customer endpoints
├── Models/
│   ├── EF/                          # Entity Framework models
│   │   ├── NorthwindContext.cs      # DbContext for Northwind database
│   │   ├── Customer.cs              # Customer entity
│   │   ├── Order.cs                 # Order entity
│   │   ├── Product.cs               # Product entity
│   │   └── [other entities...]
│   └── MVC/                         # Controllers in wrong folder (legacy)
├── Assets/
│   └── Northwind.db                 # SQLite database file
├── Program.cs                       # Application entry point & configuration
├── appsettings.json                 # Configuration (Serilog, JWT, ConnectionStrings)
├── .env.example                     # Template for .env file (committed to Git)
├── docker-compose.yml               # Docker Compose configuration
├── Dockerfile                       # Multi-stage Docker build configuration
├── .dockerignore                    # Docker build exclusions
├── .gitignore                       # Git exclusions (includes .env)
├── render.yaml                      # Render.com deployment configuration
└── .github/
    └── copilot-instructions.md      # This file
```

### API Endpoints

#### System Endpoints
| Endpoint | Purpose |
|----------|---------|
| `GET /` | Home page with app info and links |
| `GET /health` | Basic health check (built-in) |
| `GET /health/live` | Liveness probe for Kubernetes/containers |
| `GET /health/ready` | Readiness probe with dependency checks |
| `GET /version` | Application version |
| `GET /appname` | Application product name (text/plain) |
| `GET /appinfo` | Comprehensive app info (JSON with version, framework, etc.) |
| `GET /config` | Runtime configuration info |
| `GET /test` | Echo endpoint for testing |
| `GET /test/error` | Throws exception to demo Problem Details |

#### Authentication Endpoints
| Endpoint | Purpose |
|----------|---------|
| `POST /api/auth/login` | Login with username/password, returns JWT tokens |
| `POST /api/auth/refresh` | Refresh access token using refresh token |
| `POST /api/auth/logout` | Logout and invalidate refresh token |
| `POST /api/auth/logout-all` | Logout from all devices |
| `GET /api/auth/me` | Get current user info (requires auth) |

**Demo Users:**
- Username: `admin`, Password: `admin`, Role: `Admin`
- Username: `user`, Password: `user`, Role: `User`

#### Public Customer Endpoints (No Authentication)
| Endpoint | Purpose |
|----------|---------|
| `GET /api/public/customers` | Get all customers |
| `GET /api/public/customers/{id}` | Get customer by ID |
| `GET /api/public/customers/{id}/orders` | Get customer with orders |
| `POST /api/public/customers` | Create new customer |
| `PUT /api/public/customers/{id}` | Update customer |
| `DELETE /api/public/customers/{id}` | Delete customer |

#### Protected Customer Endpoints (Requires Authentication)
| Endpoint | Purpose |
|----------|---------|
| `GET /api/customers` | Get all customers (requires JWT) |

### JWT Configuration

JWT settings are configured in `appsettings.json`:
```json
{
  "Jwt": {
    "Secret": "default-docker-secret-change-in-production-min-32-chars-long!",
    "Issuer": "Northwind.App.Backend",
    "Audience": "Northwind.App.Frontend",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

**Configuration Hierarchy:**
1. `appsettings.json` - Default values (committed to Git)
2. `.env` file - Local development overrides (ignored by Git)
3. Environment variables - Production/Docker (highest priority)

**Important**: 
- In production, use environment variables for the JWT secret: `Jwt__Secret=your-secret`
- Never commit `.env` files to Git (use `.env.example` as template)
- JWT Secret must be minimum 32 characters long

### Database Configuration

- **Database**: SQLite (file-based, included in Assets folder)
- **Connection String**: `Data Source=Assets/Northwind.db`
- **Schema**: Classic Northwind database (Customers, Orders, Products, etc.)
- **Access**: Read-only in container (file system is ephemeral)

### Container & Deployment

#### Docker
- **Dockerfile**: Multi-stage build (build, publish, final)
- **Base Images**: 
  - Build: `mcr.microsoft.com/dotnet/sdk:10.0`
  - Runtime: `mcr.microsoft.com/dotnet/aspnet:10.0`
- **Security**: Runs as non-root user (UID/GID 1001)
- **Port**: Configurable via `PORT` environment variable (default: 8080)
- **Health Check**: Built-in HEALTHCHECK directive using `/health/live` endpoint
- **Size**: Optimized with layer caching and minimal runtime image
- **Build Quality**: Uses `--warnaserror` flag - deployment fails if any warnings exist

#### Docker Compose
- **Configuration**: `docker-compose.yml` for local development
- **Port**: 5033 (matches launchSettings.json)
- **Environment**: Loads from `.env` file automatically
- **Health Check**: Configured to check `/health/live` every 30s

#### Render.com Deployment
- **Configuration**: `render.yaml` (Blueprint)
- **Health Check**: `/health/live`
- **Environment Variables**: JWT settings configured via Render dashboard
- **Region**: Frankfurt (configurable in render.yaml)
- **Plan**: Free tier supported (spins down after 15 min inactivity)

To deploy:
1. Push code to GitHub
2. Connect repository to Render.com
3. Use Blueprint deployment (auto-detects render.yaml)
4. Application will be available at `https://[app-name].onrender.com`

### Swagger/OpenAPI

Access interactive API documentation at: `/swagger`

Features:
- All endpoints documented with XML comments
- Request/response schemas
- Try-it-out functionality
- JWT authentication support (use "Authorize" button with Bearer token)

### Logging Configuration

Serilog is configured to log to console (stdout) for container compatibility:
- **Minimum Level**: Information
- **Override**: Microsoft/System namespaces set to Warning
- **Format**: Structured logging with context
- **Output**: Console sink only (collected by container runtime)
- **Smart Filtering**: Health check requests (`/health/*`) logged at Debug level

**Request Logging Levels:**
- Health check endpoints (`/health`, `/health/live`, `/health/ready`): `Debug` level
- 5xx server errors: `Error` level
- 4xx client errors: `Warning` level
- Successful requests (2xx/3xx): `Information` level

This prevents health check spam in logs while preserving visibility of actual API usage.

**To see Debug logs (e.g., health checks):**
```bash
# In .env or environment variable
Serilog__MinimumLevel__Default=Debug
```

Example logging:
```csharp
_logger.LogInformation("User {Username} logged in successfully", username);
```

### Error Handling

- Global exception handler returns RFC 7807 Problem Details
- All exceptions automatically converted to proper HTTP responses
- Custom validation errors also return Problem Details format
- Unauthorized access returns 401 with Problem Details

### CORS Policy

Configured to allow all origins (for demo purposes):
```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
```

**Note**: In production, restrict to specific origins.

## When Modifying This Project

1. **Keep all text in English** - code, comments, logs, documentation
2. **Zero warnings policy** - Code must compile without any warnings (enforced by `--warnaserror` in Dockerfile)
3. **Add `[ProducesResponseType]` attributes** to all controller actions for Swagger
4. **Use structured logging**: `_logger.LogInformation("Message {Parameter}", value)`
5. **Add XML documentation comments** (`///`) to public API methods
6. **Follow existing patterns**:
   - SystemController for system endpoints
   - AuthController for authentication
   - CustomersController for authenticated endpoints
7. **Use `AsNoTracking()`** for read-only EF queries
8. **Use `[Authorize]`** attribute for protected endpoints
9. **Environment variables**: Use `.env.example` as template, never commit `.env`
10. **Version updates**: Update version in `.csproj` `<Version>` tag, auto-reflected in `/appinfo`
11. **Test endpoints** in Swagger UI before committing
12. **Check analyzer warnings** - Fix any Meziantou.Analyzer warnings before committing
13. **Update this file** when adding new features or patterns

## Security Considerations

- ✅ JWT tokens with configurable expiration
- ✅ Refresh token rotation
- ✅ Non-root container user
- ✅ HTTPS response compression
- ✅ .env files excluded from Git
- ✅ Safe default JWT secret for Docker (must be changed in production)
- ⚠️ Demo uses in-memory token storage (use database in production)
- ⚠️ Demo users are hardcoded (use proper user management in production)
- ⚠️ JWT secret must be changed via environment variables in production
- ⚠️ CORS is wide open (restrict in production)
