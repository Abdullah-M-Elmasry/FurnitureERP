using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Application.Inventory.DTOs;

public class InventoryTransactionDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public InventoryTransactionType Type { get; set; }

    public decimal Quantity { get; set; }

    public decimal BalanceAfter { get; set; }

    public InventoryReferenceType? ReferenceType { get; set; }

    public StockAdjustmentReason? AdjustmentReason { get; set; }

    public int? ReferenceId { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Notes { get; set; }

    public string? CreatedBy { get; set; }

    public string ReasonDisplay =>
    Type == InventoryTransactionType.Adjustment
        ? AdjustmentReason?.ToString() ?? "-"
        : "-";

   // public string ReferenceDisplay =>
   //Type == InventoryReferenceType.Adjustment
   //    ? InventoryReferenceType?.ToString() ?? "-"
   //    : "-";
}