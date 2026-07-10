namespace FurnitureERP.Application.Inventory.DTOs;

public class InventorySummaryDto
{
    public int TotalProducts { get; set; }

    public decimal TotalStock { get; set; }

    public int LowStockCount { get; set; }

    public int OutOfStockCount { get; set; }
}