# Northwind.App.Backend

En moderne ASP.NET Core REST API backend-applikation, der fungerer som en demo og reference-implementering af bedste praksis. Bygget med .NET 10, Entity Framework Core og JWT-autentificering demonstrerer dette projekt en komplet web-API med Docker-understøttelse og cloud-deployment.

## 🌟 Funktioner

- ✅ **RESTful API** - Komplette CRUD-operationer for Northwind database-entiteter
- ✅ **JWT-autentificering** - Sikre endpoints med access- og refresh-tokens
- ✅ **Entity Framework Core** - SQLite-database med code-first tilgang
- ✅ **OpenAPI/Swagger** - Interaktiv API-dokumentation
- ✅ **Struktureret Logging** - Serilog med smart filtrering (health checks logges på Debug niveau)
- ✅ **Environment Variables** - Fuld understøttelse af .env filer med DotNetEnv
- ✅ **Version Management** - Dynamisk app name og version fra assembly metadata
- ✅ **Health Checks** - Kubernetes-klar liveness og readiness probes
- ✅ **Problem Details (RFC 7807)** - Konsistente fejlsvar
- ✅ **Docker-understøttelse** - Multi-stage build med sikkerhedsbedste praksis og healthcheck
- ✅ **Cloud Ready** - Deployed på Render.com med automatiske deployments

## 🚀 Live Demo

API'et er deployed og tilgængeligt på:

**🔗 [https://northwind-backend-b088.onrender.com](https://northwind-backend-b088.onrender.com)**

> **⚠️ Bemærkning om Render.com Free Tier:**  
> Denne applikation hostes på Render's gratis tier, som automatisk lukker ned efter 15 minutters inaktivitet. Den første forespørgsel efter inaktivitet kan tage **30-50 sekunder** at svare, mens tjenesten starter op igen. Efterfølgende forespørgsler vil være hurtige. Dette er normal adfærd for free-tier deployments.

### Hurtige Links

- **Swagger UI**: [https://northwind-backend-b088.onrender.com/swagger](https://northwind-backend-b088.onrender.com/swagger)
- **Health Check**: [https://northwind-backend-b088.onrender.com/health/live](https://northwind-backend-b088.onrender.com/health/live)
- **API Version**: [https://northwind-backend-b088.onrender.com/version](https://northwind-backend-b088.onrender.com/version)
- **App Info**: [https://northwind-backend-b088.onrender.com/appinfo](https://northwind-backend-b088.onrender.com/appinfo)

## 🛠️ Teknologi Stack

- **[.NET 10](https://dotnet.microsoft.com/)** - Nyeste .NET framework
- **[ASP.NET Core Web API](https://docs.microsoft.com/aspnet/core/)** - Højtydende web framework
- **[Entity Framework Core](https://docs.microsoft.com/ef/core/)** - Moderne ORM til .NET
- **[SQLite](https://www.sqlite.org/)** - Letvægts filbaseret database
- **[JWT Bearer Authentication](https://jwt.io/)** - Industristandard token-autentificering
- **[Serilog](https://serilog.net/)** - Struktureret logging-bibliotek
- **[Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - OpenAPI/Swagger-værktøjer
- **[Meziantou.Analyzer](https://www.meziantou.net/meziantou-analyzer.htm)** - Code quality analyzer
- **[Docker](https://www.docker.com/)** - Containeriserings-platform
- **[Render.com](https://render.com/)** - Cloud-platform til deployment

## 📚 API Endpoints

### System Endpoints

| Endpoint        | Method   | Beskrivelse                           | Kræver Auth |
| --------------- | -------- | ------------------------------------- | ----------- |
| `/`             | GET      | Forside med app info og links         | Nej         |
| `/health`       | GET      | Basis health check                    | Nej         |
| `/health/live`  | GET/HEAD | Liveness probe                        | Nej         |
| `/health/ready` | GET/HEAD | Readiness probe                       | Nej         |
| `/version`      | GET      | API version                           | Nej         |
| `/appname`      | GET      | Applikationsnavn (text/plain)         | Nej         |
| `/appinfo`      | GET      | Komplet app info (JSON)               | Nej         |
| `/config`       | GET      | Runtime-konfiguration                 | Nej         |
| `/test`         | GET      | Echo test endpoint                    | Nej         |
| `/test/error`   | GET      | Test fejlhåndtering (Problem Details) | Nej         |
| `/swagger`      | GET      | API-dokumentation                     | Nej         |

### Autentificerings-Endpoints

| Endpoint               | Method | Beskrivelse            | Kræver Auth |
| ---------------------- | ------ | ---------------------- | ----------- |
| `/api/auth/login`      | POST   | Login med credentials  | Nej         |
| `/api/auth/refresh`    | POST   | Forny access token     | Nej         |
| `/api/auth/logout`     | POST   | Logout aktuel session  | Ja          |
| `/api/auth/logout-all` | POST   | Logout alle sessioner  | Ja          |
| `/api/auth/me`         | GET    | Hent aktuel brugerinfo | Ja          |

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

### Offentlige Kunde-Endpoints (Ingen Autentificering)

| Endpoint                             | Method | Beskrivelse                                    | Query Parameters                              |
| ------------------------------------ | ------ | ---------------------------------------------- | --------------------------------------------- |
| `/api/public/customers`              | GET    | Hent alle kunder (pagineret)                   | `skip` (standard: 0), `take` (standard: 1000) |
| `/api/public/customers-with-revenue` | GET    | Hent alle kunder med omsætningsinfo (sorteret) | `skip` (standard: 0), `take` (standard: 1000) |
| `/api/public/customers/{id}`         | GET    | Hent kunde efter ID                            | -                                             |
| `/api/public/customers/{id}/orders`  | GET    | Hent kunde med ordrer                          | `maxOrders` (standard: 10)                    |
| `/api/public/customers`              | POST   | Opret ny kunde                                 | -                                             |
| `/api/public/customers/{id}`         | PUT    | Opdater kunde                                  | -                                             |
| `/api/public/customers/{id}`         | PATCH  | Delvist opdater kunde                          | -                                             |
| `/api/public/customers/{id}`         | DELETE | Slet kunde                                     | -                                             |

### Beskyttede Kunde-Endpoints (Kræver Autentificering)

| Endpoint         | Method | Beskrivelse                   | Query Parameters                              |
| ---------------- | ------ | ----------------------------- | --------------------------------------------- |
| `/api/customers` | GET    | Hent alle kunder (kræver JWT) | `skip` (standard: 0), `take` (standard: 1000) |

## 🏃 Kom i Gang

### Forudsætninger

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Git](https://git-scm.com/)
- (Valgfrit) [Docker Desktop](https://www.docker.com/products/docker-desktop)
- (Valgfrit) [Visual Studio Code](https://code.visualstudio.com/) eller [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Lokal Udvikling

1. **Klon repository**
   ```bash
   git clone https://github.com/devcronberg/Northwind.App.Backend.git
   cd Northwind.App.Backend
   ```

2. **Gendan dependencies**
   ```bash
   dotnet restore
   ```

3. **(Valgfrit) Opret .env fil til lokal udvikling**
   ```bash
   cp .env.example .env
   ```
   Rediger `.env` og tilpas værdier efter behov. Filen ignoreres af Git.

4. **Kør applikationen**
   ```bash
   dotnet run
   # eller med hot reload:
   dotnet watch
   ```

5. **Åbn Swagger UI**
   
   Naviger til: [http://localhost:5033/swagger](http://localhost:5033/swagger)

### Test API'et

#### Eksempel 1: Hent alle kunder (ingen autentificering)
```bash
curl http://localhost:5000/api/public/customers
```

#### Eksempel 2: Login og få JWT token
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

Svar:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123...",
  "expiresIn": 3600
}
```

#### Eksempel 3: Tilgå beskyttet endpoint med JWT
```bash
curl http://localhost:5000/api/customers \
  -H "Authorization: Bearer DIN_ACCESS_TOKEN"
```

## 🐳 Docker

### Option 1: Brug Pre-built Image fra GitHub Container Registry (Anbefalet)

Dette projekt publicerer automatisk Docker images til GitHub Container Registry ved hver commit til `main` branch.

**Hurtigt kom i gang:**
```bash
# Pull latest image
docker pull ghcr.io/devcronberg/northwind.app.backend:latest

# Kør container
docker run -d --name northwind-api -p 5033:5033 -e ASPNETCORE_URLS=http://+:5033 ghcr.io/devcronberg/northwind.app.backend:latest

# Tilgå API'et på http://localhost:5033
# Swagger dokumentation: http://localhost:5033/swagger
```

**Med Docker Compose (anbefalet til udvikling):**

Opret en `docker-compose.yml` fil:
```yaml
services:
  northwind-api:
    image: ghcr.io/devcronberg/northwind.app.backend:latest
    container_name: northwind-backend
    ports:
      - "5033:5033"
    environment:
      - ASPNETCORE_URLS=http://+:5033
      - ASPNETCORE_ENVIRONMENT=Production
    restart: unless-stopped
```

Start med:
```bash
docker compose up -d
```

**Tilgængelige image tags:**
- `latest` - Nyeste version fra main branch
- `0.9.0` - Specifik version (fra .csproj Version tag)
- `main-sha-abc123` - Commit-specific version

### Option 2: Byg Docker Image Selv

**Fra source code:**
```bash
# Klon repository
git clone https://github.com/devcronberg/Northwind.App.Backend.git
cd Northwind.App.Backend

# Byg image
docker build -t northwind-backend .

# Kør container
docker run -d --name northwind-api \
  -p 5033:5033 \
  -e ASPNETCORE_URLS=http://+:5033 \
  northwind-backend
```

**Eller brug Docker Compose (inkluderet i repository):**
```bash
docker compose up -d --build
```

### Docker Image Detaljer

- **Multi-stage build** - Optimeret til størrelse og sikkerhed
- **Non-root bruger** - Kører som `appuser` (UID 1001)
- **Størrelse** - Cirka 220MB (kun runtime)
- **Base images**:
  - Build: `mcr.microsoft.com/dotnet/sdk:10.0`
  - Runtime: `mcr.microsoft.com/dotnet/aspnet:10.0`
- **Build kvalitet** - Bruger `--warnaserror` flag, så deployment fejler hvis der er warnings
- **Health Check** - Built-in healthcheck via `/health/live` endpoint
- **Automatisk publicering** - GitHub Actions bygger og pusher til GHCR ved hver commit

### Nyttige Docker Kommandoer

```bash
# Se kørende containers
docker ps

# Se logs
docker logs northwind-api

# Se logs live
docker logs -f northwind-api

# Stop container
docker stop northwind-api

# Start container igen
docker start northwind-api

# Fjern container
docker rm northwind-api

# Pull nyeste version
docker pull ghcr.io/devcronberg/northwind.app.backend:latest

# Opdater til nyeste version
docker compose pull && docker compose up -d
```

## ☁️ Deployment til Render.com

Dette projekt inkluderer en `render.yaml` konfigurationsfil til nem deployment til [Render.com](https://render.com/).

### Hvad er Render.com?

Render er en moderne cloud-platform, der gør det nemt at deploye web-applikationer, API'er, databaser og mere. Den tilbyder:

- **Free Tier** - Perfekt til demoer og små projekter
- **Automatiske Deployments** - Deployer automatisk når du pusher til GitHub
- **Docker-understøttelse** - Native Docker container deployment
- **HTTPS som standard** - Gratis SSL-certifikater
- **Health Checks** - Indbygget overvågning
- **Zero DevOps** - Ingen server-administration påkrævet

### Deploy til Render

1. **Push til GitHub**
   ```bash
   git add .
   git commit -m "Initial commit"
   git push origin main
   ```

2. **Opret Render-konto**
   - Tilmeld dig på [render.com](https://render.com)
   - Tilslut din GitHub-konto

3. **Deploy med Blueprint**
   - Klik "New +" → "Blueprint"
   - Vælg dette repository
   - Render vil detektere `render.yaml` og konfigurere automatisk
   - Klik "Apply"

4. **Vent på deployment** (5-10 minutter første gang)
   - Build logs viser Docker build fremskridt
   - Tjenesten vil være tilgængelig på `https://your-app-name.onrender.com`

### Render Free Tier Adfærd

Den gratis tier inkluderer:
- ✅ 750 timer/måned runtime
- ✅ Automatisk HTTPS
- ✅ Automatiske deployments fra GitHub
- ⚠️ Lukker ned efter 15 minutters inaktivitet
- ⚠️ Cold start tager 30-50 sekunder

**Tip:** Til produktion, opgrader til en betalt plan ($7/måned) for at eliminere nedlukning og få:
- Altid-på tjeneste (ingen cold starts)
- Mere RAM og CPU
- Hurtigere builds
- Support

## ⚙️ Konfiguration

Konfiguration håndteres gennem en hierarkisk struktur:
1. **appsettings.json** - Default værdier (committed til Git)
2. **.env fil** - Lokal udvikling overrides (ignoreret af Git)
3. **Environment variables** - Produktion og Docker (højeste prioritet)

### Konfigurationshierarki

Værdier fra senere kilder overskriver tidligere:

```
appsettings.json → .env fil → Environment Variables → Kommandolinje-argumenter
```

### .env Fil Support

Projektet bruger [DotNetEnv](https://github.com/tonerdo/dotnet-env) til at indlæse `.env` filer ved opstart.

**Lokal udvikling:**
```bash
# Kopier example fil
cp .env.example .env

# Rediger .env med dine lokale værdier
nano .env
```

**Eksempel .env fil:**
```bash
# JWT Configuration - overrides appsettings.json
Jwt__Secret=my-local-development-secret-key-min-32-chars
Jwt__AccessTokenExpirationMinutes=120
Jwt__RefreshTokenExpirationDays=14

# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5033

# Logging Level (optional)
# Serilog__MinimumLevel__Default=Debug
```

**Docker med docker-compose.yml:**
```bash
# Start med .env fil
docker-compose up
```

### JWT Indstillinger

**Default i appsettings.json:**
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

### Miljøvariabler (Produktion på Render.com)

**Tilføj i Render Dashboard under "Environment":**

```bash
# JWT Konfiguration (KRITISK - skift secret!)
Jwt__Secret=your-strong-production-secret-min-32-chars-long!
Jwt__Issuer=Northwind.App.Backend
Jwt__Audience=Northwind.App.Frontend

# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Production

# Logging (valgfri - reducer logs fra health checks)
Serilog__MinimumLevel__Default=Information
```

### Logging Konfiguration

Applikationen bruger Serilog med smart request logging:

- **Health check endpoints** (`/health`, `/health/live`, `/health/ready`) logges på `Debug` niveau
- **Normale requests** logges på `Information` niveau
- **4xx fejl** logges på `Warning` niveau  
- **5xx fejl** logges på `Error` niveau

**For at se Debug logs (f.eks. health checks):**
```bash
# I .env eller environment variable
Serilog__MinimumLevel__Default=Debug
```

**⚠️ Sikkerhedsbemærkninger:**
- ❌ Commit ALDRIG `.env` filer eller secrets til Git
- ✅ Brug `.env.example` som template (uden sensitive data)
- ✅ Skift altid JWT Secret i produktion
- ✅ Brug minimum 32 tegn i JWT Secret
- ✅ Brug environment variables på cloud platforms

## 📁 Projektstruktur

```
Northwind.App.Backend/
├── Controllers/                      # API Controllers
│   ├── SystemController.cs           # System endpoints
│   ├── AuthController.cs             # Autentificering
│   ├── CustomersController.cs        # Beskyttede endpoints
│   └── PublicCustomersController.cs  # Offentlige endpoints
├── Models/
│   ├── EF/                           # Entity Framework modeller
│   │   ├── NorthwindContext.cs       # Database context
│   │   ├── Customer.cs               # Kunde entitet
│   │   ├── Order.cs                  # Ordre entitet
│   │   └── [andre entiteter]
│   └── MVC/                          # Legacy mappestruktur
├── Assets/
│   └── Northwind.db                  # SQLite database
├── Program.cs                        # Applikations entry point
├── appsettings.json                  # Konfiguration
├── Dockerfile                        # Docker build konfiguration
├── docker-compose.yml                # Lokal Docker Compose setup
├── .dockerignore                     # Docker ekskluderinger
├── render.yaml                       # Render.com deployment config
└── .github/
    ├── workflows/
    │   └── docker-publish.yml        # GitHub Actions workflow
    └── copilot-instructions.md       # AI assistent instruktioner
```

## 🤖 Continuous Integration (CI/CD)

Projektet bruger **GitHub Actions** til automatisk build og publicering af Docker images.

### Automatisk Docker Image Publishing

Ved hver push til `main` branch:
1. GitHub Actions builder Docker imaget
2. Imaget pushes til GitHub Container Registry (GHCR)
3. Tilgængeligt på: `ghcr.io/devcronberg/northwind.app.backend`

**Tags der oprettes automatisk:**
- `latest` - Nyeste version
- `0.9.0` - Version fra .csproj
- `main-sha-abc123` - Git commit SHA

### GitHub Actions Workflow

Workflowet [.github/workflows/docker-publish.yml](.github/workflows/docker-publish.yml):
- ✅ Bygger Docker image med multi-stage build
- ✅ Ekstraerer version fra .csproj automatisk
- ✅ Tagger imaget med version, latest og commit SHA
- ✅ Pusher til GitHub Container Registry
- ✅ Fejler hvis der er compiler warnings (`--warnaserror`)

**Se build status:**
- Actions tab: [https://github.com/devcronberg/Northwind.App.Backend/actions](https://github.com/devcronberg/Northwind.App.Backend/actions)
- Package: [https://github.com/devcronberg/Northwind.App.Backend/pkgs/container/northwind.app.backend](https://github.com/devcronberg/Northwind.App.Backend/pkgs/container/northwind.app.backend)

**Manuelt trigger workflow:**
```bash
# Via GitHub web interface: Actions → Build and Push Docker Image → Run workflow
# Eller via GitHub CLI:
gh workflow run docker-publish.yml
```

## 🔐 Autentificering

Dette API bruger JWT (JSON Web Tokens) til autentificering.

### Autentificerings-flow

1. **Login** - POST credentials til `/api/auth/login`
2. **Modtag Tokens** - Få `accessToken` og `refreshToken`
3. **Brug Access Token** - Inkluder i `Authorization: Bearer {token}` header
4. **Forny Token** - Når access token udløber, brug refresh token til at få en ny
5. **Logout** - Invalider tokens via `/api/auth/logout`

### Brug JWT i Swagger

1. Klik "Authorize" knappen i Swagger UI
2. Indtast: `Bearer DIN_ACCESS_TOKEN`
3. Klik "Authorize"
4. Test beskyttede endpoints

### Token Udløb

- **Access Token**: 60 minutter (konfigurerbar)
- **Refresh Token**: 7 dage (konfigurerbar)

## 🧪 Test

### Brug af Swagger UI

Den nemmeste måde at teste API'et på:

1. Naviger til `/swagger`
2. Prøv de offentlige endpoints (ingen autentificering nødvendig)
3. Login via `/api/auth/login` for at få et JWT token
4. Klik "Authorize" og indsæt tokenet
5. Test beskyttede endpoints

### Brug af cURL

Se eksempler i "Test API'et" sektionen ovenfor.

### Brug af Postman

1. Importer API'et ved at indsætte Swagger JSON URL'en:
   ```
   https://northwind-backend-b088.onrender.com/swagger/v1/swagger.json
   ```
2. Opret et environment med `baseUrl` variabel
3. Test endpoints med autentificerings-flow

## 📝 Demonstrerede Best Practices

Dette projekt demonstrerer:

- ✅ **Clean Architecture** - Separation of concerns
- ✅ **Async/Await** - Korrekte async programmeringsmønstre
- ✅ **Fejlhåndtering** - Problem Details (RFC 7807) standard
- ✅ **Sikkerhed** - JWT autentificering, non-root Docker bruger
- ✅ **Logging** - Struktureret logging med Serilog
- ✅ **Dokumentation** - OpenAPI/Swagger med XML kommentarer
- ✅ **Health Checks** - Kubernetes-klar probes
- ✅ **CORS** - Konfigureret til cross-origin requests
- ✅ **Docker** - Multi-stage builds, layer caching
- ✅ **Cloud Native** - Container-klar, 12-factor app principper
- ✅ **Environment Variables** - DotNetEnv for .env fil support
- ✅ **Smart Logging** - Health check logs filtreres til Debug niveau
- ✅ **Code Quality** - Meziantou.Analyzer for best practices enforcement
- ✅ **Zero Warnings** - Docker build fejler ved compiler warnings (`--warnaserror`)
- ✅ **Version Management** - Assembly metadata for app name og version
- ✅ **CI/CD** - GitHub Actions automatisk Docker image publishing
- ✅ **Container Registry** - Automatisk publicering til GitHub Container Registry

## 🤝 Bidrag

Dette er et demo-projekt til læringsformål. Du er velkommen til at:

- Forke repository
- Oprette feature branches
- Indsende pull requests
- Rapportere issues
- Foreslå forbedringer

## 📄 Licens

Dette projekt er open source og tilgængeligt til uddannelsesformål.

## 🙏 Anerkendelser

- **Northwind Database** - Klassisk sample database fra Microsoft
- **ASP.NET Core Team** - For det fremragende framework
- **Render.com** - For nem cloud hosting

## 📞 Kontakt & Support

- **Repository**: [https://github.com/devcronberg/Northwind.App.Backend](https://github.com/devcronberg/Northwind.App.Backend)
- **Live Demo**: [https://northwind-backend-b088.onrender.com](https://northwind-backend-b088.onrender.com)
- **Dokumentation**: Tilgængelig på `/swagger` endpoint

---

**God Kodning! 🚀**

*Dette er en demo-applikation til uddannelsesformål. Til produktion, implementer ordentlig brugerstyring, database persistens, rate limiting og sikkerhedshærdning.*
