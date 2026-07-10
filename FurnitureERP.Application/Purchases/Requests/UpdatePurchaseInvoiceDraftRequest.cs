namespace FurnitureERP.Application.Purchases.Requests;

public class UpdatePurchaseInvoiceDraftRequest
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public int SupplierId { get; set; }

    public string? Notes { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public List<UpdatePurchaseInvoiceItemRequest> Items
    {
        get;
        set;
    } = new();
}