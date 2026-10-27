using Microsoft.Extensions.Logging;
using SupplyFlow.Application.Contracts;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Domain.Entities;

namespace SupplyFlow.Application.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
}

public sealed class ProductService(IProductRepository repository, ILogger<ProductService> logger) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(Map).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException("Product", id);
        return Map(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Sku))
            throw new BusinessRuleException("SKU is required.");
        if (request.Price <= 0)
            throw new BusinessRuleException("Price must be positive.");
        if (request.StockQuantity < 0)
            throw new BusinessRuleException("Stock cannot be negative.");
        if (await repository.SkuExistsAsync(request.Sku.Trim().ToUpperInvariant(), cancellationToken))
            throw new BusinessRuleException($"SKU '{request.Sku}' already exists.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Sku = request.Sku.Trim().ToUpperInvariant(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = string.IsNullOrWhiteSpace(request.Category) ? "General" : request.Category.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await repository.AddAsync(product, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product {ProductId} created with SKU {Sku}", product.Id, product.Sku);
        return Map(product);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException("Product", id);
        if (request.Price <= 0)
            throw new BusinessRuleException("Price must be positive.");

        product.Name = request.Name.Trim();
        product.Description = request.Description?.Trim() ?? string.Empty;
        product.Price = request.Price;
        product.Category = request.Category.Trim();
        product.IsArchived = request.IsArchived;
        product.UpdatedAt = DateTimeOffset.UtcNow;

        await repository.SaveChangesAsync(cancellationToken);
        return Map(product);
    }

    private static ProductDto Map(Product product) =>
        new(product.Id, product.Sku, product.Name, product.Price, product.StockQuantity, product.Category, product.IsArchived);
}
