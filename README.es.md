**Languages:**  
[English](./README.md) | [Русский](./README.ru.md) | [Español](./README.es.md)

---

# Servicio Backend CV

Servicio backend para la aplicación CV (Curriculum Vitae) construido con ASP.NET Core.

El proyecto se utiliza como la parte backend de una plataforma de portafolio y como demostración de decisiones arquitectónicas y de ingeniería.  
Proporciona una API pública para el frontend, gestión de perfiles y generación de currículum en PDF.

---

## 🛠 Stack Tecnológico

- **Plataforma:** .NET 10 (ASP.NET Core)
- **Lenguaje:** C# 14
- **Bases de datos:**
    - PostgreSQL — almacenamiento principal
    - MongoDB — datos de perfil en formato documento
- **Almacenamiento de archivos:** MinIO (compatible con S3)
- **Autenticación:** JWT
- **Login externo:** Google OAuth
- **Generación de PDF:** Playwright (Chromium)
- **Contenerización:** Docker / Docker Compose
- **Logging:** Structured Logging (Serilog)

---

## 🧱 Arquitectura

El proyecto está construido con énfasis en:
- principios de Clean Architecture
- separación de responsabilidades
- límites estrictos entre capas

Capas principales:
- **Domain** — modelo de negocio y reglas
- **Application** — casos de uso, DTOs, validación
- **Infrastructure** — bases de datos, almacenamiento de archivos, PDF, autenticación
- **Presentation (Web API)** — API HTTP, middleware

---

## 🚦 Estado Actual

El backend está funcionalmente completo y es utilizado por el frontend.

- escenarios públicos y autenticados implementados
- la generación de currículum en PDF funciona en producción
- las decisiones arquitectónicas están fijadas

Los cambios futuros se esperan únicamente como mejoras puntuales si son necesarias.  
El foco principal del desarrollo se ha desplazado al frontend.

---

## ⚙️ Configuración

La aplicación se configura mediante variables de entorno.

Grupos principales de configuración:
- conexiones a PostgreSQL y MongoDB
- JWT (issuer, audience, claves, tiempo de vida del token)
- Google OAuth (client id, secret, URLs de redirección)
- MinIO / S3 (endpoint, credenciales, buckets)
- Playwright / PDF (opciones de lanzamiento de Chromium)
- parámetros de entorno y URLs públicas

Un ejemplo de configuración se encuentra en `.env.example`.

---

## 📌 Mejoras Potenciales (Backlog)

El proyecto no se encuentra en una fase de expansión activa, pero son posibles las siguientes líneas de desarrollo:

### 1. Localización (i18n)
- Ampliación del soporte de localización del contenido del perfil
- Idiomas adicionales para los datos de usuario

### 2. Observabilidad
- Health Checks
- Uso ampliado de OpenTelemetry (tracing, métricas)

### 3. Seguridad
- Rate limiting para endpoints públicos
- Mecanismos adicionales de protección de la API bajo mayor carga

### 4. Rendimiento
- Caché de datos públicos solicitados con frecuencia
- Optimización adicional de consultas

---