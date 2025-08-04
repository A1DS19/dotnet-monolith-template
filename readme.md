# DMT API - .NET 9 Minimal API Template

A production-ready .NET 9 minimal API template with clean architecture, CQRS pattern, and comprehensive development tooling for rapid development and prototyping.

## Features

### **Architecture & Patterns**
- **.NET 9 Minimal APIs** - Lightweight, fast HTTP APIs with minimal ceremony
- **Clean Architecture** - Domain-driven layered architecture with proper dependency management
- **CQRS with MediatR** - Command Query Responsibility Segregation for better separation of concerns
- **Repository Pattern** - Data access layer abstraction with Dapper ORM
- **Carter Framework** - Minimal API organization and routing

### **Development Tools**
- **Docker Compose** - Complete development environment with hot reload
- **Hot Reload** - Real-time code changes without container restart
- **API Versioning** - Built-in versioning support (v1, v2, etc.)
- **OpenAPI/Swagger** - Auto-generated API documentation with Scalar UI
- **Entity Framework Tools** - Database migrations and tooling

### **Observability & Logging**
- **Serilog** - Structured logging with multiple sinks
- **Seq Integration** - Centralized log aggregation and analysis
- **Performance Monitoring** - Request timing and slow query detection
- **Configurable Logging** - Environment-specific logging levels and data capture
- **MediatR Behaviors** - Cross-cutting concerns (logging, validation, etc.)

### **Data & Persistence**
- **SQL Server** - Primary database with Docker container
- **Redis** - Distributed caching and session storage
- **Dapper** - Lightweight ORM for high-performance data access
- **Database Migrations** - SQL scripts for schema management
- **Connection Pooling** - Optimized database connections

### **Cross-Cutting Concerns**
- **CORS** - Configurable cross-origin resource sharing
- **Global Exception Handling** - Centralized error management
- **Validation** - Input validation with proper error responses
- **Health Checks** - Service health monitoring endpoints

## Getting Started

### Prerequisites
- Docker & Docker Compose
- .NET 9 SDK (for local development)

### Quick Start
```bash
# Start all services
docker compose up

# Or run in detached mode
docker compose up -d

# View logs
docker compose logs -f api
```

### Database Setup
```bash
# Run database setup scripts
docker exec -it dmt-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C -i /path/to/Database/SetupDatabase.sql
```

## 🌐 Service URLs

| Service | URL | Credentials |
| :--- | :--- | :--- |
| **API Documentation** | `http://localhost:9090/scalar/v1` | - |
| **API Endpoints** | `http://localhost:9090/api/v1` | - |
| **Seq Logs** | `http://localhost:5341` | `admin` / `Admin123!` |
| **SQL Server** | `localhost:1433` | `sa` / `YourStrong!Passw0rd` |
| **Redis** | `localhost:6379` | Password: `YourRedisPassword123` |

## Project Structure

```
DMT.Api/                    # Presentation Layer
├── Behaviors/              # MediatR cross-cutting concerns
├── Modules/                # Carter API modules/routes
└── Program.cs              # Application bootstrap

DMT.Application/            # Application Layer
├── Features/               # CQRS commands/queries
│   └── Products/
│       ├── Commands/       # Write operations
│       └── Queries/        # Read operations
├── Dtos/                   # Data transfer objects
├── Interfaces/             # Application contracts
└── Services/               # Application services

DMT.Infrastructure/         # Infrastructure Layer
├── Data/                   # Database connection factory
├── Extensions/             # Service registrations
└── Repositories/           # Data access implementations

DMT.Domain/                 # Domain Layer
└── Entities/               # Core business entities

Database/                   # Database Scripts
├── SetupDatabase.sql       # Complete setup script
└── README.md               # Database documentation
```

## Configuration

### Environment Variables (Docker)
```yaml
# Database Connection
ConnectionStrings__DefaultConnection: "Server=sqlserver,1433;Database=DMT;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true"

# Redis Connection  
ConnectionStrings__Redis: "dmt-redis:6379,password=YourRedisPassword123"

# Seq Logging
Serilog__WriteTo__2__Args__serverUrl: "http://seq:80"

# API Configuration
ApiSettings__AllowedOrigins__0: "http://localhost:3000"
ASPNETCORE_ENVIRONMENT: "Development"
ASPNETCORE_URLS: "http://+:9090"
```

### Logging Configuration

#### Development (`appsettings.Development.json`)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    },
    "LogRequestData": true,     // Log request details
    "LogResponseData": true,    // Log response details  
    "MaxDataLength": 2000       // Max log data size
  }
}
```

#### Production (`appsettings.json`)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "LogRequestData": false,    // Disable for performance
    "LogResponseData": false,   // Disable for performance
    "MaxDataLength": 500        // Smaller log size
  }
}
```

### API Settings
```json
{
  "ApiSettings": {
    "Timeout": 30,
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://localhost:3000"
    ],
    "EnableSwagger": true,
    "EnableSensitiveDataLogging": false,
    "RateLimitRequests": 100
  }
}
```

## 🛠️ Development Commands

### Docker Operations
```bash
# Build and start services
docker compose up --build

# Rebuild specific service
docker compose build api --no-cache

# View service logs
docker compose logs api -f

# Stop services
docker compose down

# Remove volumes (reset data)
docker compose down -v
```

### Database Operations
```bash
# Connect to SQL Server
docker exec -it dmt-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C

# Run database setup
docker exec -it dmt-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -C -i /tmp/SetupDatabase.sql
```
## Monitoring & Debugging

### Seq Logs
- Access: `http://localhost:5341`
- Credentials: `admin` / `Admin123!`
- Features: Structured logging, filtering, dashboards

### Performance Monitoring
- Request/response logging with timing
- Slow query detection (>3s warning)
- Configurable log levels per environment

### Hot Reload Development
- Real-time code changes
- Automatic rebuild and restart
- Source code volume mounting

## Architecture Principles

### Dependency Flow
```
DMT.Api → DMT.Infrastructure → DMT.Application → DMT.Domain
```

### Layer Responsibilities
- **Domain**: Core business entities and rules
- **Application**: Use cases, CQRS handlers, business logic
- **Infrastructure**: Data access, external services, implementations
- **API**: HTTP endpoints, dependency injection, cross-cutting concerns

## Production Considerations

### Performance
- Configurable logging to reduce overhead
- Connection pooling for database access
- Compact JSON serialization
- Response caching strategies

### Security
- Environment-specific configurations
- Secrets management via environment variables
- CORS policy configuration
- Input validation and sanitization

### Scalability
- Stateless API design
- Redis for distributed caching
- Horizontal scaling ready
- Database connection optimization
