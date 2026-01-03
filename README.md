# Northwind.App.Backend

A modern ASP.NET Core REST API backend application serving as a demo and reference implementation for best practices. Built with .NET 10, Entity Framework Core, and JWT authentication, this project demonstrates a complete web API with Docker support and cloud deployment.

## 🌟 Features

- ✅ **RESTful API** - Complete CRUD operations for Northwind database entities
- ✅ **JWT Authentication** - Secure endpoints with access and refresh tokens
- ✅ **Entity Framework Core** - SQLite database with code-first approach
- ✅ **OpenAPI/Swagger** - Interactive API documentation
- ✅ **Structured Logging** - Serilog with console output for containers
- ✅ **Health Checks** - Kubernetes-ready liveness and readiness probes
- ✅ **Problem Details (RFC 7807)** - Consistent error responses
- ✅ **Docker Support** - Multi-stage build with security best practices
- ✅ **Cloud Ready** - Deployed on Render.com with automatic deployments

## 🚀 Live Demo

The API is deployed and accessible at:

**🔗 [https://northwind-backend-b088.onrender.com](https://northwind-backend-b088.onrender.com)**

> **⚠️ Note about Render.com Free Tier:**  
> This application is hosted on Render's free tier, which automatically spins down after 15 minutes of inactivity. The first request after inactivity may take **30-50 seconds** to respond while the service spins back up. Subsequent requests will be fast. This is normal behavior for free-tier deployments.

### Quick Links

- **Swagger UI**: [https://northwind-backend-b088.onrender.com/swagger](https://northwind-backend-b088.onrender.com/swagger)
- **Health Check**: [https://northwind-backend-b088.onrender.com/health/live](https://northwind-backend-b088.onrender.com/health/live)
- **API Version**: [https://northwind-backend-b088.onrender.com/version](https://northwind-backend-b088.onrender.com/version)

## 🛠️ Technology Stack

- **[.NET 10](https://dotnet.microsoft.com/)** - Latest .NET framework
- **[ASP.NET Core Web API](https://docs.microsoft.com/aspnet/core/)** - High-performance web framework
- **[Entity Framework Core](https://docs.microsoft.com/ef/core/)** - Modern ORM for .NET
- **[SQLite](https://www.sqlite.org/)** - Lightweight file-based database
- **[JWT Bearer Authentication](https://jwt.io/)** - Industry-standard token authentication
- **[Serilog](https://serilog.net/)** - Structured logging library
- **[Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - OpenAPI/Swagger tooling
- **[Docker](https://www.docker.com/)** - Containerization platform
- **[Render.com](https://render.com/)** - Cloud platform for deployment

## 📚 API Endpoints

### System Endpoints

| Endpoint | Method | Description | Auth Required |
|----------|--------|-------------|---------------|
| `/` | GET | Redirects to Swagger UI | No |
| `/health` | GET | Basic health check | No |
| `/health/live` | GET/HEAD | Liveness probe | No |
| `/health/ready` | GET/HEAD | Readiness probe | No |
| `/version` | GET | API version | No |
| `/config` | GET | Runtime configuration | No |
| `/test` | GET | Echo test endpoint | No |
| `/test/error` | GET | Test error handling | No |
| `/swagger` | GET | API documentation | No |

### Authentication Endpoints

| Endpoint | Method | Description | Auth Required |
|----------|--------|-------------|---------------|
| `/api/auth/login` | POST | Login with credentials | No |
| `/api/auth/refresh` | POST | Refresh access token | No |
| `/api/auth/logout` | POST | Logout current session | Yes |
| `/api/auth/logout-all` | POST | Logout all sessions | Yes |
| `/api/auth/me` | GET | Get current user info | Yes |

**Demo Credentials:**
```json
{
  "username": "admin",
  "password": "admin"
}
```
or
```json
{
  "username": "user",
  "password": "user"
}
```

### Public Customer Endpoints (No Authentication)

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/public/customers` | GET | Get all customers |
| `/api/public/customers/{id}` | GET | Get customer by ID |
| `/api/public/customers/{id}/orders` | GET | Get customer with orders |
| `/api/public/customers` | POST | Create new customer |
| `/api/public/customers/{id}` | PUT | Update customer |
| `/api/public/customers/{id}` | DELETE | Delete customer |

### Protected Customer Endpoints (Authentication Required)

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/customers` | GET | Get all customers (requires JWT) |

## 🏃 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Git](https://git-scm.com/)
- (Optional) [Docker Desktop](https://www.docker.com/products/docker-desktop)
- (Optional) [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/devcronberg/Northwind.App.Backend.git
   cd Northwind.App.Backend
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Open Swagger UI**
   
   Navigate to: [http://localhost:5000/swagger](http://localhost:5000/swagger)

### Testing the API

#### Example 1: Get all customers (no authentication)
```bash
curl http://localhost:5000/api/public/customers
```

#### Example 2: Login and get JWT token
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123...",
  "expiresIn": 3600
}
```

#### Example 3: Access protected endpoint with JWT
```bash
curl http://localhost:5000/api/customers \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

## 🐳 Docker

### Build Docker Image
```bash
docker build -t northwind-backend .
```

### Run Container
```bash
docker run -p 8080:8080 northwind-backend
```

The API will be available at [http://localhost:8080](http://localhost:8080)

### Docker Image Details

- **Multi-stage build** - Optimized for size and security
- **Non-root user** - Runs as `appuser` (UID 1001)
- **Size** - Approximately 220MB (runtime only)
- **Base images**:
  - Build: `mcr.microsoft.com/dotnet/sdk:10.0`
  - Runtime: `mcr.microsoft.com/dotnet/aspnet:10.0`

## ☁️ Deployment to Render.com

This project includes a `render.yaml` configuration file for easy deployment to [Render.com](https://render.com/).

### What is Render.com?

Render is a modern cloud platform that makes it easy to deploy web applications, APIs, databases, and more. It offers:

- **Free Tier** - Perfect for demos and small projects
- **Automatic Deployments** - Deploys automatically when you push to GitHub
- **Docker Support** - Native Docker container deployment
- **HTTPS by default** - Free SSL certificates
- **Health Checks** - Built-in monitoring
- **Zero DevOps** - No server management required

### Deploy to Render

1. **Push to GitHub**
   ```bash
   git add .
   git commit -m "Initial commit"
   git push origin main
   ```

2. **Create Render Account**
   - Sign up at [render.com](https://render.com)
   - Connect your GitHub account

3. **Deploy using Blueprint**
   - Click "New +" → "Blueprint"
   - Select this repository
   - Render will detect `render.yaml` and configure automatically
   - Click "Apply"

4. **Wait for deployment** (5-10 minutes first time)
   - Build logs will show Docker build progress
   - Service will be available at `https://your-app-name.onrender.com`

### Render Free Tier Behavior

The free tier includes:
- ✅ 750 hours/month of runtime
- ✅ Automatic HTTPS
- ✅ Automatic deployments from GitHub
- ⚠️ Spins down after 15 minutes of inactivity
- ⚠️ Cold start takes 30-50 seconds

**Tip:** For production use, upgrade to a paid plan ($7/month) to eliminate spin-down and get:
- Always-on service (no cold starts)
- More RAM and CPU
- Faster builds
- Support

## ⚙️ Configuration

Configuration is managed through `appsettings.json` and environment variables.

### JWT Settings

```json
{
  "Jwt": {
    "Secret": "YourSecretKeyHere",
    "Issuer": "Northwind.App.Backend",
    "Audience": "Northwind.App.Frontend",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Environment Variables (for Production)

```bash
# JWT Configuration
Jwt__Secret=your-production-secret-key
Jwt__Issuer=Northwind.App.Backend
Jwt__Audience=Northwind.App.Frontend

# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
```

**⚠️ Security Note:** Never commit secrets to Git. Use environment variables or secret management services in production.

## 📁 Project Structure

```
Northwind.App.Backend/
├── Controllers/                      # API Controllers
│   ├── SystemController.cs           # System endpoints
│   ├── AuthController.cs             # Authentication
│   ├── CustomersController.cs        # Protected endpoints
│   └── PublicCustomersController.cs  # Public endpoints
├── Models/
│   ├── EF/                           # Entity Framework models
│   │   ├── NorthwindContext.cs       # Database context
│   │   ├── Customer.cs               # Customer entity
│   │   ├── Order.cs                  # Order entity
│   │   └── [other entities]
│   └── MVC/                          # Legacy folder structure
├── Assets/
│   └── Northwind.db                  # SQLite database
├── Program.cs                        # Application entry point
├── appsettings.json                  # Configuration
├── Dockerfile                        # Docker build configuration
├── .dockerignore                     # Docker exclusions
├── render.yaml                       # Render.com deployment config
└── .github/
    └── copilot-instructions.md       # AI assistant instructions
```

## 🔐 Authentication

This API uses JWT (JSON Web Tokens) for authentication.

### Authentication Flow

1. **Login** - POST credentials to `/api/auth/login`
2. **Receive Tokens** - Get `accessToken` and `refreshToken`
3. **Use Access Token** - Include in `Authorization: Bearer {token}` header
4. **Refresh Token** - When access token expires, use refresh token to get new one
5. **Logout** - Invalidate tokens via `/api/auth/logout`

### Using JWT in Swagger

1. Click "Authorize" button in Swagger UI
2. Enter: `Bearer YOUR_ACCESS_TOKEN`
3. Click "Authorize"
4. Test protected endpoints

### Token Expiration

- **Access Token**: 60 minutes (configurable)
- **Refresh Token**: 7 days (configurable)

## 🧪 Testing

### Using Swagger UI

The easiest way to test the API:

1. Navigate to `/swagger`
2. Try the public endpoints (no authentication needed)
3. Login via `/api/auth/login` to get a JWT token
4. Click "Authorize" and paste the token
5. Test protected endpoints

### Using cURL

See examples in the "Testing the API" section above.

### Using Postman

1. Import the API by pasting the Swagger JSON URL:
   ```
   https://northwind-backend-b088.onrender.com/swagger/v1/swagger.json
   ```
2. Create an environment with `baseUrl` variable
3. Test endpoints with authentication flow

## 📝 Best Practices Demonstrated

This project demonstrates:

- ✅ **Clean Architecture** - Separation of concerns
- ✅ **Async/Await** - Proper async programming patterns
- ✅ **Error Handling** - Problem Details (RFC 7807) standard
- ✅ **Security** - JWT authentication, non-root Docker user
- ✅ **Logging** - Structured logging with Serilog
- ✅ **Documentation** - OpenAPI/Swagger with XML comments
- ✅ **Health Checks** - Kubernetes-ready probes
- ✅ **CORS** - Configured for cross-origin requests
- ✅ **Docker** - Multi-stage builds, layer caching
- ✅ **Cloud Native** - Container-ready, 12-factor app principles

## 🤝 Contributing

This is a demo project for learning purposes. Feel free to:

- Fork the repository
- Create feature branches
- Submit pull requests
- Report issues
- Suggest improvements

## 📄 License

This project is open source and available for educational purposes.

## 🙏 Acknowledgments

- **Northwind Database** - Classic sample database from Microsoft
- **ASP.NET Core Team** - For the excellent framework
- **Render.com** - For easy cloud hosting

## 📞 Contact & Support

- **Repository**: [https://github.com/devcronberg/Northwind.App.Backend](https://github.com/devcronberg/Northwind.App.Backend)
- **Live Demo**: [https://northwind-backend-b088.onrender.com](https://northwind-backend-b088.onrender.com)
- **Documentation**: Available at `/swagger` endpoint

---

**Happy Coding! 🚀**

*This is a demo application for educational purposes. For production use, implement proper user management, database persistence, rate limiting, and security hardening.*
