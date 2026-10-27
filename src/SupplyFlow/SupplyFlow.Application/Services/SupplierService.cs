using SupplyFlow.Application.Contracts;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Domain.Entities;

namespace SupplyFlow.Application.Services;

public interface ISupplierService
{
    Task<IReadOnlyList<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default);
}

public sealed class SupplierService(ISupplierRepository repository) : ISupplierService
{
    public async Task<IReadOnlyList<SupplierDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = await repository.GetAllAsync(cancellationToken);
        return suppliers.Select(Map).ToList();
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new BusinessRuleException("Supplier name is required.");
        if (string.IsNullOrWhiteSpace(request.ContactEmail))
            throw new BusinessRuleException("Supplier email is required.");

        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            ContactEmail = request.ContactEmail.Trim(),
            Phone = request.Phone?.Trim() ?? string.Empty,
            Country = string.IsNullOrWhiteSpace(request.Country) ? "Russia" : request.Country.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        await repository.AddAsync(supplier, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return Map(supplier);
    }

    private static SupplierDto Map(Supplier supplier) =>
        new(supplier.Id, supplier.Name, supplier.ContactEmail, supplier.Phone, supplier.Country, supplier.IsActive);
}
