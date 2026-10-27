using Microsoft.EntityFrameworkCore;
using SupplyFlow.Domain.Entities;

namespace SupplyFlow.Infrastructure.Data;

public sealed class SupplyFlowDbContext(DbContextOptions<SupplyFlowDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Supply> Supplies => Set<Supply>();
    public DbSet<SupplyItem> SupplyItems => Set<SupplyItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Id).HasColumnName("id");
            entity.Property(product => product.Sku).HasColumnName("sku").HasMaxLength(32).IsRequired();
            entity.HasIndex(product => product.Sku).IsUnique();
            entity.Property(product => product.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(product => product.Description).HasColumnName("description").HasMaxLength(2000);
            entity.Property(product => product.Price).HasColumnName("price").HasPrecision(18, 2);
            entity.Property(product => product.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(product => product.Category).HasColumnName("category").HasMaxLength(100);
            entity.Property(product => product.CreatedAt).HasColumnName("created_at");
            entity.Property(product => product.UpdatedAt).HasColumnName("updated_at");
            entity.Property(product => product.IsArchived).HasColumnName("is_archived");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("suppliers");
            entity.HasKey(supplier => supplier.Id);
            entity.Property(supplier => supplier.Id).HasColumnName("id");
            entity.Property(supplier => supplier.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(supplier => supplier.ContactEmail).HasColumnName("contact_email").HasMaxLength(320).IsRequired();
            entity.Property(supplier => supplier.Phone).HasColumnName("phone").HasMaxLength(30);
            entity.Property(supplier => supplier.Country).HasColumnName("country").HasMaxLength(100);
            entity.Property(supplier => supplier.CreatedAt).HasColumnName("created_at");
            entity.Property(supplier => supplier.IsActive).HasColumnName("is_active");
            entity.HasIndex(supplier => supplier.ContactEmail).IsUnique();
        });

        modelBuilder.Entity<Supply>(entity =>
        {
            entity.ToTable("supplies");
            entity.HasKey(supply => supply.Id);
            entity.Property(supply => supply.Id).HasColumnName("id");
            entity.Property(supply => supply.Number).HasColumnName("number").HasMaxLength(40).IsRequired();
            entity.HasIndex(supply => supply.Number).IsUnique();
            entity.Property(supply => supply.SupplierId).HasColumnName("supplier_id");
            entity.Property(supply => supply.Status).HasColumnName("status");
            entity.Property(supply => supply.CreatedAt).HasColumnName("created_at");
            entity.Property(supply => supply.SubmittedAt).HasColumnName("submitted_at");
            entity.Property(supply => supply.DeliveredAt).HasColumnName("delivered_at");
            entity.Property(supply => supply.CompletedAt).HasColumnName("completed_at");
            entity.Property(supply => supply.WarehouseCode).HasColumnName("warehouse_code").HasMaxLength(50);
            entity.Property(supply => supply.Notes).HasColumnName("notes").HasMaxLength(1000);
            entity.HasOne(supply => supply.Supplier)
                .WithMany(supplier => supplier.Supplies)
                .HasForeignKey(supply => supply.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SupplyItem>(entity =>
        {
            entity.ToTable("supply_items");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnName("id");
            entity.Property(item => item.SupplyId).HasColumnName("supply_id");
            entity.Property(item => item.ProductId).HasColumnName("product_id");
            entity.Property(item => item.Quantity).HasColumnName("quantity");
            entity.Property(item => item.UnitPrice).HasColumnName("unit_price").HasPrecision(18, 2);
            entity.HasOne(item => item.Supply)
                .WithMany(supply => supply.Items)
                .HasForeignKey(item => item.SupplyId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(item => item.Product)
                .WithMany(product => product.SupplyItems)
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(item => new { item.SupplyId, item.ProductId }).IsUnique();
        });
    }
}
