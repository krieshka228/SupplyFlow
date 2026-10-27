# SupplyFlow

Учебный pet-проект для позиций C#-стажёра: управление товарами, поставщиками и поставками.

## Технологии

- C# 12 / .NET 8
- ASP.NET Core Web API
- Blazor Server
- Entity Framework Core 8
- PostgreSQL
- xUnit
- Docker Compose
- GitHub Actions

## Быстрый старт

```bash
docker compose up -d
dotnet run --project src/SupplyFlow/SupplyFlow.Api
dotnet run --project src/SupplyFlow/SupplyFlow.Web
```

API: http://localhost:5080/swagger  
Web: http://localhost:5000

## Архитектура

- `SupplyFlow.Domain` — бизнес-сущности и правила.
- `SupplyFlow.Application` — DTO, сервисы, интерфейсы репозиториев.
- `SupplyFlow.Infrastructure` — EF Core, PostgreSQL, миграции, репозитории.
- `SupplyFlow.Api` — REST API.
- `SupplyFlow.Web` — Blazor Server UI.
- `SupplyFlow.Tests` — unit и integration тесты.

## Бизнес-процесс поставки

Draft → Submitted → InTransit → Delivered → Accepted
