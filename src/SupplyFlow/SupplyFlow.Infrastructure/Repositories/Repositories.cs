using Microsoft.EntityFrameworkCore;
using SupplyFlow.Application.Contracts;
using SupplyFlow.Domain.Entities;
using SupplyFlow.Infrastructure.Data;

namespace SupplyFlow.Infrastructure.Repositories;

public sealed class ProductRepository(SupplyFlowDbContext context) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Products.AsNoTracking().OrderBy(product => product.Name).ToListAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

    public async Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken = default) =>
        await context.Products.AnyAsync(product => product.Sku == sku, cancellationToken);

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default) =>
        await context.Products.AddAsync(product, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}

public sealed class SupplierRepository(SupplyFlowDbContext context) : ISupplierRepository
{
    public async Task<IReadOnlyList<Supplier>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Suppliers.AsNoTracking().OrderBy(supplier => supplier.Name).ToListAsync(cancellationToken);

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default) =>
        await context.Suppliers.AddAsync(supplier, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}

public sealed class SupplyRepository(SupplyFlowDbContext context) : ISupplyRepository
{
    public async Task<IReadOnlyList<Supply>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.Supplies
            .AsNoTracking()
            .Include(supply => supply.Supplier)
            .Include(supply => supply.Items)
            .ThenInclude(item => item.Product)
            .OrderByDescending(supply => supply.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<Supply?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Supplies
            .Include(supply => supply.Supplier)
            .Include(supply => supply.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(supply => supply.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Supply>> GetBySupplierAsync(Guid supplierId, CancellationToken cancellationToken = default) =>
        await context.Supplies
            .AsNoTracking()
            .Where(supply => supply.SupplierId == supplierId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Supply supply, CancellationToken cancellationToken = default) =>
        await context.Supplies.AddAsync(supply, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);
}
