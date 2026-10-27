using SupplyFlow.Domain.Entities;
using SupplyFlow.Domain.Enums;
using Xunit;

namespace SupplyFlow.Tests;

public sealed class SupplyStateMachineTests
{
    private static Supply CreateSupply()
    {
        var product = new Product { Id = Guid.NewGuid(), Sku = "SKU", Name = "Product", StockQuantity = 0 };
        var supply = new Supply
        {
            Id = Guid.NewGuid(),
            Number = "SF-TEST",
            Supplier = new Supplier { Id = Guid.NewGuid(), Name = "Supplier" }
        };
        supply.Items.Add(new SupplyItem { Product = product, Quantity = 5, UnitPrice = 100 });
        return supply;
    }

    [Fact]
    public void Accept_Should_Increase_Stock()
    {
        var supply = CreateSupply();
        supply.Submit();
        supply.StartTransit();
        supply.MarkDelivered();
        supply.Accept();

        Assert.Equal(SupplyStatus.Accepted, supply.Status);
        Assert.Equal(5, supply.Items.First().Product.StockQuantity);
    }

    [Fact]
    public void Submit_Should_Throw_When_Items_Empty()
    {
        var supply = CreateSupply();
        supply.Items.Clear();

        Assert.Throws<InvalidOperationException>(supply.Submit);
    }
}
