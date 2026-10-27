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
- REST API + Swagger
- Dashboard с ключевыми метриками

## Быстрый старт

```bash
docker compose up -d
dotnet run --project src/SupplyFlow/SupplyFlow.Web
```

Web: http://localhost:5047

API запускается отдельно:

```bash
dotnet run --project src/SupplyFlow/SupplyFlow.Api
```

Swagger: http://localhost:5080/swagger

## Важное

- PostgreSQL поднимается на порту **5433**, чтобы не конфликтовать с локальным PostgreSQL.
- Строка подключения уже настроена.

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
