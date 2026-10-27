using SupplyFlow.Domain.Enums;

namespace SupplyFlow.Domain.Entities;

public sealed class Supply
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public SupplyStatus Status { get; set; } = SupplyStatus.Draft;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public DateTimeOffset? DeliveredAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
    public string WarehouseCode { get; set; } = "WH-MOSCOW";
    public string Notes { get; set; } = string.Empty;

    public ICollection<SupplyItem> Items { get; set; } = new List<SupplyItem>();

    public void Submit()
    {
        if (Status != SupplyStatus.Draft)
            throw new InvalidOperationException("Only a draft supply can be submitted.");
        if (Items.Count == 0)
            throw new InvalidOperationException("Supply must contain at least one item.");

        Status = SupplyStatus.Submitted;
        SubmittedAt = DateTimeOffset.UtcNow;
    }

    public void StartTransit()
    {
        if (Status != SupplyStatus.Submitted)
            throw new InvalidOperationException("Only a submitted supply can be sent to transit.");
        Status = SupplyStatus.InTransit;
    }

    public void MarkDelivered()
    {
        if (Status != SupplyStatus.InTransit)
            throw new InvalidOperationException("Only an in-transit supply can be delivered.");
        Status = SupplyStatus.Delivered;
        DeliveredAt = DateTimeOffset.UtcNow;
    }

    public void Accept()
    {
        if (Status != SupplyStatus.Delivered)
            throw new InvalidOperationException("Only a delivered supply can be accepted.");

        foreach (var item in Items)
            item.Product.IncreaseStock(item.Quantity);

        Status = SupplyStatus.Accepted;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Reject(string reason)
    {
        if (Status is not (SupplyStatus.Submitted or SupplyStatus.InTransit or SupplyStatus.Delivered))
            throw new InvalidOperationException("This supply cannot be rejected in the current status.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reject reason is required.", nameof(reason));

        Status = SupplyStatus.Rejected;
        Notes = reason;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        if (Status is not (SupplyStatus.Draft or SupplyStatus.Submitted))
            throw new InvalidOperationException("This supply cannot be cancelled in the current status.");
        Status = SupplyStatus.Cancelled;
        CompletedAt = DateTimeOffset.UtcNow;
    }
}

