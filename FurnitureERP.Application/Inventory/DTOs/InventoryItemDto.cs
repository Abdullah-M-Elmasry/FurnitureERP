public class InventoryItemDto
{
    private const decimal LowStockLimit = 5;

    public int ProductId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string? Barcode { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Unit { get; set; } = string.Empty;

    public decimal CurrentStock { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsOutOfStock =>
        CurrentStock == 0;

    public bool IsLowStock =>
        CurrentStock > 0 &&
        CurrentStock <= LowStockLimit;

    public bool IsAvailable =>
        CurrentStock > LowStockLimit;

    public string Status =>
        IsOutOfStock
            ? "Out Of Stock"
            : IsLowStock
                ? "Low Stock"
                : "Available";
}