using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Application.Inventory.DTOs;

public class StockAdjustmentDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public StockAdjustmentReason Reason { get; set; }

    public string? Notes { get; set; }
}