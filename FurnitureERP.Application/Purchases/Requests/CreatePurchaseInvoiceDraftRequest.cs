//using FurnitureERP.Domain.Entities.Purchases;

namespace FurnitureERP.Application.Purchases.Requests;

public class CreatePurchaseInvoiceDraftRequest
{
    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public int SupplierId { get; set; }

    public string? Notes { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public List<CreatePurchaseInvoiceItemRequest> Items
    {
        get;
        set;
    } = new();

    
}