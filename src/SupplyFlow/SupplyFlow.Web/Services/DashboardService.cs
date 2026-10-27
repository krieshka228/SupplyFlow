using Microsoft.EntityFrameworkCore;
using SupplyFlow.Domain.Enums;
using SupplyFlow.Infrastructure.Data;

namespace SupplyFlow.Web.Services;

public sealed record DashboardSummary(
    int ProductCount,
    int SupplierCount,
    int ActiveSupplyCount,
    int LowStockCount,
    decimal AcceptedSupplyValue);

public sealed class DashboardService(SupplyFlowDbContext context)
{
    public async Task<DashboardSummary> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var products = await context.Products.AsNoTracking().ToListAsync(cancellationToken);
        var suppliers = await context.Suppliers.AsNoTracking().CountAsync(cancellationToken);
        var supplies = await context.Supplies.AsNoTracking()
            .Include(supply => supply.Items)
            .ToListAsync(cancellationToken);

        return new DashboardSummary(
            products.Count,
            suppliers,
            supplies.Count(supply => supply.Status is SupplyStatus.Draft or SupplyStatus.Submitted or SupplyStatus.InTransit or SupplyStatus.Delivered),
            products.Count(product => product.StockQuantity <= 10),
            supplies.Where(supply => supply.Status == SupplyStatus.Accepted)
                .SelectMany(supply => supply.Items)
                .Sum(item => item.Quantity * item.UnitPrice));
    }
}
