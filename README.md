**Languages:**  
[English](./README.md) | [Русский](./README.ru.md) | [Español](./README.es.md)

---

# CV Backend Service

Backend service for the CV (Curriculum Vitae) application built with ASP.NET Core.

The project is used as the backend part of a portfolio platform and serves as a demonstration of architectural and engineering decisions.  
It provides a public API for the frontend, profile management, and resume generation.

---

## 🛠 Tech Stack

- **Platform:** .NET 10 (ASP.NET Core)
- **Language:** C# 14
- **Databases:**
  - PostgreSQL
  - MongoDB
- **File Storage:** MinIO (S3-compatible)
- **Authentication:** JWT
- **External Login:** Google OAuth
- **PDF Generation:** Playwright (Chromium)
- **Containerization:** Docker / Docker Compose
- **Logging:** Structured Logging (Serilog)

---

## 🧱 Architecture

The project is built with a focus on:
- Clean Architecture principles
- separation of responsibilities
- strict boundaries between layers

Main layers:
- **Domain** — business model and rules
- **Application** — use cases, DTOs, validation
- **Infrastructure** — databases, file storage, PDF, authentication
- **Presentation (Web API)** — HTTP API, middleware

---

## 🚦 Current Status

The backend is functionally complete and actively used by the frontend.

- public and authenticated scenarios are implemented
- resume PDF generation works in production
- architectural decisions are fixed

Further changes are expected only as targeted improvements if needed.
The main development focus has shifted to the frontend.

---

## ⚙️ Configuration

The application is configured via environment variables.

Main configuration groups:
- PostgreSQL and MongoDB connections
- JWT (issuer, audience, keys, token lifetime)
- Google OAuth (client id, secret, redirect URLs)
- MinIO / S3 (endpoint, credentials, buckets)
- Playwright / PDF (Chromium launch options)
- environment and public URL parameters

An example configuration is provided in `.env.example`.

---

## 📌 Potential Improvements (Backlog)

The project is not in an active expansion phase, but the following directions are possible:

### 1. Localization (i18n)
- Extended localization support for profile content
- Additional languages for user data

### 2. Observability
- Health Checks
- Extended OpenTelemetry usage (tracing, metrics)

### 3. Security
- Rate limiting for public endpoints
- Additional API protection mechanisms under higher load

### 4. Performance
- Caching of frequently requested public data
- Further query optimizations

---