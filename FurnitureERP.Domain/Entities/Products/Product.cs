using FurnitureERP.Domain.Entities.Inventories;

namespace FurnitureERP.Domain.Entities.Products;

public class Product
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string? Barcode { get; set; }
    public string Name { get; set; } = string.Empty;

    public decimal CostPrice { get; set; }

    public decimal SalePrice { get; set; }

   
    public bool IsActive { get; set; } = true;

    public decimal MinimumStock { get; set; } = 2;

    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int CategoryId { get; set; }
    public ProductCategory Category { get; set; } = null!;

    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;

    public ProductInventory? Inventory { get; set; }

    public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
        = new List<InventoryTransaction>();
}