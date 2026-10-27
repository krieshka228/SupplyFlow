namespace SupplyFlow.Application.DTOs;

public sealed record SupplierDto(
    Guid Id,
    string Name,
    string ContactEmail,
    string Phone,
    string Country,
    bool IsActive);

public sealed class CreateSupplierRequest
{
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = "Россия";
}
