using FurnitureERP.Domain.Entities.Suppliers;
using FurnitureERP.Domain.Enums;

public class PurchaseInvoice
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = "";

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public int SupplierId { get; set; }

    public Supplier Supplier { get; set; } = null!;

    public string? Notes { get; set; }

    public PurchaseInvoiceStatus Status { get; set; }
        = PurchaseInvoiceStatus.Draft;

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    public string? ConfirmedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public ICollection<PurchaseInvoiceItem> Items
        = new List<PurchaseInvoiceItem>();

    public decimal SubTotal { get; set; }

    public decimal GrandTotal { get; set; }
}