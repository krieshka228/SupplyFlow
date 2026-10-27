using SupplyFlow.Application.Contracts;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Domain.Entities;
using SupplyFlow.Domain.Enums;

namespace SupplyFlow.Application.Services;

public interface ISupplyWorkflowService
{
    Task ResetDemoDataAsync(CancellationToken cancellationToken = default);
}

public sealed class SupplyWorkflowService(ISupplyWorkflowRepository repository) : ISupplyWorkflowService
{
    public async Task ResetDemoDataAsync(CancellationToken cancellationToken = default)
    {
        await repository.ResetAsync(cancellationToken);

        var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Sku = "SKU-1001", Name = "Клавиатура механическая", Description = "RGB, синие переключатели", Price = 6490, StockQuantity = 8, Category = "Электроника", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Sku = "SKU-1002", Name = "Мышь беспроводная", Description = "USB-C, 8000 DPI", Price = 3290, StockQuantity = 12, Category = "Электроника", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Sku = "SKU-1003", Name = "Монитор 27\"", Description = "QHD, 165 Гц", Price = 32990, StockQuantity = 5, Category = "Электроника", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Sku = "SKU-2001", Name = "Кофе в зернах", Description = "1 кг, обжарка средняя", Price = 1790, StockQuantity = 20, Category = "Продукты", CreatedAt = DateTimeOffset.UtcNow }
        };

        var suppliers = new List<Supplier>
        {
            new() { Id = Guid.NewGuid(), Name = "ТехноПоставка", ContactEmail = "sales@technopost.ru", Phone = "+7 495 111-22-33", Country = "Россия", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "FreshFoods", ContactEmail = "orders@freshfoods.ru", Phone = "+7 495 444-55-66", Country = "Россия", CreatedAt = DateTimeOffset.UtcNow }
        };

        var draft = new Supply
        {
            Id = Guid.NewGuid(),
            Number = "SF-DEMO-0001",
            SupplierId = suppliers[0].Id,
            Supplier = suppliers[0],
            Status = SupplyStatus.Draft,
            WarehouseCode = "WH-MOSCOW",
            Notes = "Черновик новой поставки",
            CreatedAt = DateTimeOffset.UtcNow
        };
        draft.Items.Add(new SupplyItem { Id = Guid.NewGuid(), Product = products[0], ProductId = products[0].Id, Quantity = 10, UnitPrice = 5100 });

        var transit = new Supply
        {
            Id = Guid.NewGuid(),
            Number = "SF-DEMO-0002",
            SupplierId = suppliers[1].Id,
            Supplier = suppliers[1],
            Status = SupplyStatus.InTransit,
            WarehouseCode = "WH-MOSCOW",
            Notes = "Первая демо-поставка",
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-3),
            SubmittedAt = DateTimeOffset.UtcNow.AddDays(-2)
        };
        transit.Items.Add(new SupplyItem { Id = Guid.NewGuid(), Product = products[3], ProductId = products[3].Id, Quantity = 25, UnitPrice = 1350 });

        await repository.SeedAsync(products, suppliers, new List<Supply> { draft, transit }, cancellationToken);
    }
}
