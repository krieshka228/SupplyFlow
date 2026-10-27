# Архитектура SupplyFlow

## Общая схема

```text
Browser
  ↓
Blazor Server (SupplyFlow.Web)
  ↓
Application Services
  ↓
Repositories
  ↓
EF Core
  ↓
PostgreSQL
```

API используется отдельным приложением и может вызываться интеграциями.

## Слои

- **Domain**: сущности `Product`, `Supplier`, `Supply`, `SupplyItem` и статусная машина поставки.
- **Application**: DTO, интерфейсы репозиториев, сервисы прикладной логики, валидация.
- **Infrastructure**: `DbContext`, репозитории, миграции EF Core, PostgreSQL.
- **Api**: REST контроллеры и Swagger.
- **Web**: Blazor Server UI.
- **Tests**: unit-тесты домена и сервисов.

## Ключевые принципы

- Dependency Inversion: Application не знает про EF Core.
- Repository Pattern: изолирует доступ к данным.
- DTO: наружу отдаём только нужные поля.
- Статусная машина: запрещает некорректные переходы.
- Миграции: версия схемы БД в git.

## Docker

- `postgres` — база данных, healthcheck.
- `api` — REST API.
- `web` — Blazor Server.
- Все сервисы в сети `supplyflow`.
- Миграции применяются при старте приложения.
- При пустой базе автоматически создаются демо-данные.
- Data Protection ключи хранятся в отдельных томах.
- Приложения работают от непривилегированного пользователя.
