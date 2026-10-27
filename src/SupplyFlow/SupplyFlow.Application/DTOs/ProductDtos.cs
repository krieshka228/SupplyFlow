namespace SupplyFlow.Application.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    int StockQuantity,
    string Category,
    bool IsArchived);

public sealed class CreateProductRequest
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Category { get; set; } = "General";
}

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string Category,
    bool IsArchived);
