# SupplyFlow

Учебный pet-проект для позиций C#-стажёра: система управления товарами, поставщиками, поставками и товарными остатками.

## Технологии

- C# 12 / .NET 8
- ASP.NET Core Web API
- Blazor Server
- Entity Framework Core 8
- PostgreSQL
- xUnit
- Docker Compose
- GitHub Actions

## Функциональность

- Товары: создание, список, остатки, категории, цена, SKU
- Поставщики: создание, список
- Поставки: создание с позициями, изменение статуса, сумма поставки
- Статусная машина: `Draft → Submitted → InTransit → Delivered → Accepted`
- Автоматическое пополнение остатков при приёмке поставки
- Кнопка загрузки демо-данных
- Автозаполнение демо-данными при пустой базе
- REST API + Swagger
- Dashboard с ключевыми метриками
- Healthcheck для Web и API

## Быстрый старт

### Через Docker

```bash
cp .env.example .env
docker compose up --build -d
```

Приложение:

- Web: http://localhost:5047
- API: http://localhost:5080
- Swagger: http://localhost:5080/swagger
- PostgreSQL: `localhost:5433`

### Локально

```bash
docker compose up -d postgres
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$HOME/.dotnet:$PATH"
dotnet run --project src/SupplyFlow/SupplyFlow.Web
dotnet run --project src/SupplyFlow/SupplyFlow.Api
```

## Архитектура

- `SupplyFlow.Domain` — бизнес-сущности и правила.
- `SupplyFlow.Application` — DTO, сервисы, интерфейсы репозиториев.
- `SupplyFlow.Infrastructure` — EF Core, PostgreSQL, миграции, репозитории.
- `SupplyFlow.Api` — REST API.
- `SupplyFlow.Web` — Blazor Server UI.
- `SupplyFlow.Tests` — unit-тесты.

## Бизнес-процесс поставки

```text
Draft → Submitted → InTransit → Delivered → Accepted
```

При `Accept` остатки товаров автоматически увеличиваются.

## Тесты

```bash
dotnet test SupplyFlow.sln
```

## Производственный запуск

```bash
docker compose -f docker-compose.yml up --build -d
```

- Healthcheck PostgreSQL
- Приложения с restart policy
- Единая Docker-сеть
- Конфигурация через переменные окружения
- Миграции применяются автоматически при старте
- Автозаполнение демо-данными при пустой базе
- Healthcheck для PostgreSQL, Web и API
- Контейнеры запускаются от непривилегированного пользователя
- Data Protection ключи сохраняются в отдельные Docker-тома

## Переменные окружения

| Переменная | Значение по умолчанию | Назначение |
|---|---:|---|
| `POSTGRES_USER` | `postgres` | Пользователь PostgreSQL |
| `POSTGRES_PASSWORD` | `postgres` | Пароль PostgreSQL |
| `POSTGRES_DB` | `supplyflow` | Имя базы данных |
| `ConnectionStrings__Default` | см. `docker-compose.yml` | Строка подключения |
