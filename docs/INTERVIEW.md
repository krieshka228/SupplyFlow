# Вопросы и ответы для собеседования

## Что делает SupplyFlow?

Система управления товарами, поставщиками и поставками. Поддерживает жизненный цикл поставки и изменение остатков.

## Почему .NET 8?

LTS-версия, актуальная для вакансий, поддерживает C# 12 и EF Core 8.

## Зачем слои?

Domain хранит бизнес-правила, Application — сценарии, Infrastructure — EF Core и PostgreSQL, Api/Web — транспорт и UI. Это упрощает тестирование и замену реализации.

## Как работает статусная машина?

`Draft → Submitted → InTransit → Delivered → Accepted`. Каждый метод проверяет текущий статус и выбрасывает исключение при запрещённом переходе.

## Что происходит при Accept?

Увеличивается `StockQuantity` у всех товаров поставки, ставится статус `Accepted`.

## Зачем репозитории?

Чтобы сервисы не зависели от EF Core, можно тестировать на InMemory и заменить доступ к данным.

## Как запустить?

```bash
docker compose up -d
dotnet run --project src/SupplyFlow/SupplyFlow.Api
dotnet run --project src/SupplyFlow/SupplyFlow.Web
```
