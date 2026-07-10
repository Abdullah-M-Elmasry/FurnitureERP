
//using FurnitureERP.Domain.Entities.Inventorys;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Domain.Enums;



namespace FurnitureERP.Domain.Entities.Inventories
{
    public class InventoryTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }

        public InventoryTransactionType Type { get; set; }

        public StockAdjustmentReason? AdjustmentReason { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;

        // المستخدم الذي قام بالحركة
        public string? CreatedBy { get; set; }

        public string? Notes { get; set; }

        public int? ReferenceId { get; set; }

        public decimal BalanceAfter { get; set; }

        public InventoryReferenceType? ReferenceType { get; set; }
        public Product Product { get; set; } = null!;


    }

   
}