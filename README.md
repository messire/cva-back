**Languages:**  
[English](./README.md) | [Русский](./README.ru.md) | [Español](./README.es.md)

---

# CV Backend Service

Backend service for a CV (Curriculum Vitae) application built with ASP.NET Core.
Designed to manage user data, professional experience, and CV generation.

---

## 🛠 Tech Stack

- **Platform:** .NET 10 (ASP.NET Core)
- **Language:** C# 14
- **Databases:**
  - **PostgreSQL**
  - **MongoDB**
- **Containerization:** Docker (multi-stage builds, non-root user)
- **API Documentation:** Swagger / Scalar (OpenAPI)
- **Validation:** FluentValidation
- **Testing:** xUnit, Moq, Testcontainers (integration tests)

---

## 🚀 Quick Start

The service can be launched in a production-like Docker environment using helper scripts:

- `Makefile` — for Linux / macOS / WSL
- `run.ps1` — for Windows PowerShell

The scripts automatically start the backend and the selected database based on the configuration in `.env`.

### 🗄 Database Selection

The database type is defined by the `Database__Type` parameter in the `.env` file:
- `Database__Type=Postgres` — starts PostgreSQL and pgAdmin containers
- `Database__Type=Mongo` — starts MongoDB and Mongo Express containers

The service will be available at: `http://localhost:8080`

The database selection is performed exclusively via configuration and does not require any code changes.

---

## ⚙️ Configuration

All settings are configured via environment variables in the `.env` file.

Main parameters:
- `Database__Type`: Selected database type (Postgres/Mongo)
- `Database__Postgres__Connection`: PostgreSQL connection string
- `Database__Mongo__Connection`: MongoDB connection string

---

## 📈 Potential Improvements (Backlog)

The project is under active development. Possible areas for further improvement and evolution include:

### 1. Error Handling and API Standardization
- **Problem Details (RFC 7807):** Standardized API error responses
- **Global Exception Middleware:** Centralized exception handling with contextual logging

### 2. Localization (i18n)
- Multi-language support for CV content
- Localization of validation error messages

### 3. Observability
- Health checks
- OpenTelemetry (tracing, metrics)
- Structured logging (Serilog)

### 4. Security
- Authentication (JWT / IdentityServer)
- CORS configuration
- Rate limiting

### 5. Performance
- Caching
- Query optimization
- Response compression

### 6. Database Support Expansion
- **SQLite:** Adding SQLite support to simplify local development and execution without deploying heavy databases (PostgreSQL/MongoDB).
