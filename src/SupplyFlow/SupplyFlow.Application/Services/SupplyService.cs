using Microsoft.Extensions.Logging;
using SupplyFlow.Application.Contracts;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Domain.Entities;
using SupplyFlow.Domain.Enums;

namespace SupplyFlow.Application.Services;

public interface ISupplyService
{
    Task<IReadOnlyList<SupplyDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SupplyDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplyDto> CreateAsync(CreateSupplyRequest request, CancellationToken cancellationToken = default);
    Task<SupplyDto> SubmitAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplyDto> SendToTransitAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplyDto> MarkDeliveredAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplyDto> AcceptAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplyDto> RejectAsync(Guid id, string reason, CancellationToken cancellationToken = default);
    Task<SupplyDto> CancelAsync(Guid id, CancellationToken cancellationToken = default);
}

public sealed class SupplyService(
    ISupplyRepository supplyRepository,
    ISupplierRepository supplierRepository,
    IProductRepository productRepository,
    ILogger<SupplyService> logger) : ISupplyService
{
    public async Task<IReadOnlyList<SupplyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var supplies = await supplyRepository.GetAllAsync(cancellationToken);
        return supplies.Select(Map).ToList();
    }

    public async Task<SupplyDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var supply = await supplyRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException("Supply", id);
        return Map(supply);
    }

    public async Task<SupplyDto> CreateAsync(CreateSupplyRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items.Count == 0)
            throw new BusinessRuleException("Supply must contain at least one item.");
        if (request.Items.Any(item => item.Quantity <= 0))
            throw new BusinessRuleException("Item quantity must be positive.");
        if (request.Items.Any(item => item.UnitPrice <= 0))
            throw new BusinessRuleException("Unit price must be positive.");

        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new EntityNotFoundException("Supplier", request.SupplierId);

        var supply = new Supply
        {
            Id = Guid.NewGuid(),
            Number = $"SF-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}",
            SupplierId = supplier.Id,
            Supplier = supplier,
            WarehouseCode = string.IsNullOrWhiteSpace(request.WarehouseCode) ? "WH-MOSCOW" : request.WarehouseCode.Trim().ToUpperInvariant(),
            Notes = request.Notes?.Trim() ?? string.Empty,
            CreatedAt = DateTimeOffset.UtcNow,
            Status = SupplyStatus.Draft
        };

        foreach (var itemRequest in request.Items)
        {
            var product = await productRepository.GetByIdAsync(itemRequest.ProductId, cancellationToken)
                ?? throw new EntityNotFoundException("Product", itemRequest.ProductId);

            supply.Items.Add(new SupplyItem
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Product = product,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.UnitPrice
            });
        }

        await supplyRepository.AddAsync(supply, cancellationToken);
        await supplyRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Supply {SupplyNumber} created for supplier {SupplierId}", supply.Number, supplier.Id);
        return Map(supply);
    }

    public async Task<SupplyDto> SubmitAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteTransitionAsync(id, supply => supply.Submit(), cancellationToken);

    public async Task<SupplyDto> SendToTransitAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteTransitionAsync(id, supply => supply.StartTransit(), cancellationToken);

    public async Task<SupplyDto> MarkDeliveredAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteTransitionAsync(id, supply => supply.MarkDelivered(), cancellationToken);

    public async Task<SupplyDto> AcceptAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteTransitionAsync(id, supply => supply.Accept(), cancellationToken);

    public async Task<SupplyDto> RejectAsync(Guid id, string reason, CancellationToken cancellationToken = default) =>
        await ExecuteTransitionAsync(id, supply => supply.Reject(reason), cancellationToken);

    public async Task<SupplyDto> CancelAsync(Guid id, CancellationToken cancellationToken = default) =>
        await ExecuteTransitionAsync(id, supply => supply.Cancel(), cancellationToken);

    private async Task<SupplyDto> ExecuteTransitionAsync(
        Guid id,
        Action<Supply> action,
        CancellationToken cancellationToken)
    {
        var supply = await supplyRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException("Supply", id);

        action(supply);
        await supplyRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Supply {SupplyId} moved to status {Status}", id, supply.Status);
        return Map(supply);
    }

    private static SupplyDto Map(Supply supply) => new(
        supply.Id,
        supply.Number,
        supply.SupplierId,
        supply.Supplier?.Name ?? string.Empty,
        supply.Status,
        supply.CreatedAt,
        supply.WarehouseCode,
        supply.Items.Select(item => new SupplyItemDto(
            item.ProductId,
            item.Product?.Name ?? string.Empty,
            item.Quantity,
            item.UnitPrice)).ToList(),
        supply.Items.Sum(item => item.Quantity * item.UnitPrice));
}
