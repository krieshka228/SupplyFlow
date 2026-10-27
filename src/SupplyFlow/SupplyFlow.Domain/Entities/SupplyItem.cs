namespace SupplyFlow.Domain.Entities;

public sealed class SupplyItem
{
    public Guid Id { get; set; }
    public Guid SupplyId { get; set; }
    public Supply Supply { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
