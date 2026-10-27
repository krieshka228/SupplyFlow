namespace SupplyFlow.Application.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    int StockQuantity,
    string Category,
    bool IsArchived);

public sealed record CreateProductRequest(
    string Sku,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string Category);

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string Category,
    bool IsArchived);
