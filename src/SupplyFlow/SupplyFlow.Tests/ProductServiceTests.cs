using Microsoft.EntityFrameworkCore;
using SupplyFlow.Application.Contracts;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Application.Services;
using SupplyFlow.Domain.Entities;
using SupplyFlow.Infrastructure.Data;
using SupplyFlow.Infrastructure.Repositories;
using Xunit;

namespace SupplyFlow.Tests;

public sealed class ProductServiceTests
{
    private static ProductService CreateService()
    {
        var options = new DbContextOptionsBuilder<SupplyFlowDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new SupplyFlowDbContext(options);
        IProductRepository repository = new ProductRepository(context);
        return new ProductService(repository, NullLogger<ProductService>.Instance);
    }

    [Fact]
    public async Task Create_Should_Add_Product()
    {
        var service = CreateService();
        var product = await service.CreateAsync(new CreateProductRequest { Sku = "sku-1", Name = "Monitor", Description = "27 inch", Price = 15000, StockQuantity = 10, Category = "Electronics" }, TestContext.Current.CancellationToken);

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("SKU-1", product.Sku);
    }

    [Fact]
    public async Task Create_Should_Reject_Duplicate_Sku()
    {
        var service = CreateService();
        await service.CreateAsync(new CreateProductRequest { Sku = "sku-1", Name = "Monitor", Description = "", Price = 15000, StockQuantity = 10, Category = "Electronics" }, TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.CreateAsync(new CreateProductRequest { Sku = "sku-1", Name = "Second", Price = 100, StockQuantity = 1, Category = "Electronics" }, TestContext.Current.CancellationToken));
    }
}
