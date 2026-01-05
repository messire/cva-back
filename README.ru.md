**Languages:**  
[English](./README.md) | [Русский](./README.ru.md) | [Español](./README.es.md)

---

# CV Backend Service

Backend-сервис для приложения CV (Curriculum Vitae), построенный на ASP.NET Core
Предназначен для управления данными пользователей, их профессиональным опытом и генерации CV

---

## 🛠 Технологический стек

- **Платформа:** .NET 10 (ASP.NET Core)
- **Язык:** C# 14
- **Базы данных:**
  - **PostgreSQL**
  - **MongoDB**
- **Контейнеризация:** Docker (multi-stage builds, non-root user)
- **API Documentation:** Swagger / Scalar (OpenAPI)
- **Валидация:** FluentValidation
- **Тестирование:** xUnit, Moq, Testcontainers (интеграционные тесты)

---

## 🚀 Быстрый запуск

Для запуска сервиса в Docker-окружении (production-like режим) используются вспомогательные скрипты:

- `Makefile` — для Linux / macOS / WSL
- `run.ps1` — для Windows PowerShell

Скрипты автоматически поднимают backend и выбранную базу данных на основе настроек в `.env`

### Команды (Makefile / PowerShell)

| Описание | Makefile | PowerShell (Windows) |
| :--- | :--- | :--- |
| Сборка и запуск в фоне | `make up` | `./run.ps1 up` |
| Просмотр логов | `make logs` | `./run.ps1 logs` |
| Остановка и удаление | `make down` | `./run.ps1 down` |
| Перезапуск | `make restart` | `./run.ps1 restart` |
| Статус контейнеров | `make ps` | `./run.ps1 ps` |
| Очистка ресурсов Docker | `make clean` | `./run.ps1 clean` |

### 🗄 Выбор базы данных
Тип используемой базы данных определяется параметром Database__Type в файле .env:
- `Database__Type=Postgres` — запустит контейнеры PostgreSQL и pgAdmin
- `Database__Type=Mongo` — запустит контейнеры MongoDB и Mongo Express

Сервис будет доступен по адресу: `http://localhost:8080`

Выбор базы данных осуществляется исключительно через конфигурацию и не требует изменений в коде приложения.

---

## ⚙️ Конфигурация

Все настройки осуществляются через переменные окружения в файле `.env`

Основные параметры:
- `Database__Type`: Тип используемой БД (Postgres/Mongo)
- `Database__Postgres__Connection`: Строка подключения к PostgreSQL
- `Database__Mongo__Connection`: Строка подключения к MongoDB

---

## 📈 Потенциальные улучшения (Backlog)

Проект находится в активной разработке. Ниже перечислены возможные направления дальнейшего улучшения и развития:

### 1. Обработка ошибок и стандартизация API
- **Problem Details (RFC 7807):** Внедрение стандарта для возврата единообразных ошибок API
- **Global Exception Middleware:** Централизованная обработка исключений с логированием контекста

### 2. Локализация (i18n)
- Поддержка нескольких языков для контента CV
- Локализация сообщений об ошибках валидации

### 3. Наблюдаемость (Observability)
- Health Checks
- OpenTelemetry (tracing, metrics)
- Structured Logging (Serilog)

### 4. Безопасность
- Внедрение аутентификации (JWT / IdentityServer)
- Настройка CORS политик
- Rate Limiting

### 5. Производительность
- Кеширование
- Оптимизация запросов
- Сжатие ответов

### 6. Расширение поддержки БД
- **SQLite:** Добавление поддержки SQLite для упрощения локальной разработки и запуска без необходимости развертывания тяжелых БД (PostgreSQL/MongoDB).
