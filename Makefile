.PHONY: build up down restart logs ps clean

# Переменные
ENV_FILE = .env
BACK_COMPOSE = docker/docker-compose.back.yml

# Определяем тип базы данных из .env (удаляем возможные пробелы и символы \r)
DB_TYPE := $(shell grep Database__Type $(ENV_FILE) | cut -d'=' -f2 | tr -d '\r ' )

# Формируем список файлов compose (для команд, которые должны работать со всеми)
DB_COMPOSE =
ifeq ($(DB_TYPE),Postgres)
    DB_COMPOSE = docker/docker-compose.postgres.yml
endif
ifeq ($(DB_TYPE),Mongo)
    DB_COMPOSE = docker/docker-compose.mongo.yml
endif

# Создание сети если её нет
network:
	docker network inspect docker-network-shared >/dev/null 2>&1 || docker network create docker-network-shared

# Сборка и запуск
up: network
	@if [ -n "$(DB_COMPOSE)" ]; then \
		docker compose -f $(DB_COMPOSE) --env-file $(ENV_FILE) up -d; \
	fi
	docker compose -f $(BACK_COMPOSE) --env-file $(ENV_FILE) up -d --build

# Остановка
down:
	docker compose -f $(BACK_COMPOSE) down
	@if [ -n "$(DB_COMPOSE)" ]; then \
		docker compose -f $(DB_COMPOSE) down; \
	fi

# Перезагрузка
restart: down up

# Сборка без запуска
build:
	docker compose -f $(BACK_COMPOSE) build

# Просмотр логов
logs:
	docker compose -f $(BACK_COMPOSE) logs -f

# Статус контейнеров
ps:
	docker compose -f $(BACK_COMPOSE) ps
	@if [ -n "$(DB_COMPOSE)" ]; then \
		docker compose -f $(DB_COMPOSE) ps; \
	fi

# Очистка неиспользуемых образов и контейнеров
clean:
	docker system prune -f
