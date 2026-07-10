using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Domain.Entities.Sales;

public class SalesInvoice
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = "";

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public string? Notes { get; set; }

    public SalesInvoiceStatus Status { get; set; }
        = SalesInvoiceStatus.Draft;

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    public string? ConfirmedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public ICollection<SalesInvoiceItem> Items
        = new List<SalesInvoiceItem>();

    public decimal SubTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal RemainingAmount
    {
        get => GrandTotal - PaidAmount;
    }
}