using SupplyFlow.Domain.Enums;

namespace SupplyFlow.Application.DTOs;

public sealed record SupplyItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice);

public sealed record SupplyDto(
    Guid Id,
    string Number,
    Guid SupplierId,
    string SupplierName,
    SupplyStatus Status,
    DateTimeOffset CreatedAt,
    string WarehouseCode,
    IReadOnlyCollection<SupplyItemDto> Items,
    decimal TotalAmount);

public sealed record CreateSupplyRequest(
    Guid SupplierId,
    string WarehouseCode,
    string Notes,
    IReadOnlyCollection<CreateSupplyItemRequest> Items);

public sealed record CreateSupplyItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice);

public sealed record SupplyActionRequest(string? Reason);
