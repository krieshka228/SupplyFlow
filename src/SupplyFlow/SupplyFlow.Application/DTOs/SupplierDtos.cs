namespace SupplyFlow.Application.DTOs;

public sealed record SupplierDto(
    Guid Id,
    string Name,
    string ContactEmail,
    string Phone,
    string Country,
    bool IsActive);

public sealed record CreateSupplierRequest(
    string Name,
    string ContactEmail,
    string Phone,
    string Country);
