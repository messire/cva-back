**Languages:**  
[English](./README.md) | [Русский](./README.ru.md) | [Español](./README.es.md)

---

# CV Backend Service

Servicio backend para una aplicación de CV (Curriculum Vitae) construido con ASP.NET Core.
Diseñado para gestionar datos de usuarios, experiencia profesional y la generación de CV.

---

## 🛠 Stack Tecnológico

- **Plataforma:** .NET 10 (ASP.NET Core)
- **Lenguaje:** C# 14
- **Bases de datos:**
  - **PostgreSQL**
  - **MongoDB**
- **Contenerización:** Docker (multi-stage builds, non-root user)
- **Documentación de API:** Swagger / Scalar (OpenAPI)
- **Validación:** FluentValidation
- **Testing:** xUnit, Moq, Testcontainers (pruebas de integración)

---

## 🚀 Inicio Rápido

El servicio puede ejecutarse en un entorno Docker de tipo production-like utilizando scripts auxiliares:

- `Makefile` — para Linux / macOS / WSL
- `run.ps1` — para Windows PowerShell

Los scripts inician automáticamente el backend y la base de datos seleccionada según la configuración en `.env`.

### 🗄 Selección de Base de Datos

El tipo de base de datos se define mediante el parámetro `Database__Type` en el archivo `.env`:
- `Database__Type=Postgres` — inicia contenedores de PostgreSQL y pgAdmin
- `Database__Type=Mongo` — inicia contenedores de MongoDB y Mongo Express

El servicio estará disponible en: `http://localhost:8080`

La selección de la base de datos se realiza exclusivamente mediante configuración y no requiere cambios en el código.

---

## ⚙️ Configuración

Todas las configuraciones se realizan a través de variables de entorno definidas en el archivo `.env`.

Parámetros principales:
- `Database__Type`: Tipo de base de datos seleccionada (Postgres/Mongo)
- `Database__Postgres__Connection`: Cadena de conexión a PostgreSQL
- `Database__Mongo__Connection`: Cadena de conexión a MongoDB

---

## 📈 Mejoras Potenciales (Backlog)

El proyecto se encuentra en desarrollo activo. Las posibles áreas de mejora incluyen:

### 1. Manejo de Errores y Estandarización de la API
- **Problem Details (RFC 7807):** Respuestas de error estandarizadas
- **Global Exception Middleware:** Manejo centralizado de excepciones con logging contextual

### 2. Localización (i18n)
- Soporte multilenguaje para el contenido del CV
- Localización de mensajes de validación

### 3. Observabilidad
- Health checks
- OpenTelemetry (tracing, métricas)
- Logging estructurado (Serilog)

### 4. Seguridad
- Autenticación (JWT / IdentityServer)
- Configuración de CORS
- Rate limiting

### 5. Rendimiento
- Caché
- Optimización de consultas
- Compresión de respuestas

### 6. Expansión de Soporte de Bases de Datos
- **SQLite:** Añadir soporte para SQLite para simplificar el desarrollo y la ejecución local sin dependencias externas (PostgreSQL/MongoDB).
