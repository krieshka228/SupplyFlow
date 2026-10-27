namespace SupplyFlow.Domain.Entities;

public sealed class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = "Russia";
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
