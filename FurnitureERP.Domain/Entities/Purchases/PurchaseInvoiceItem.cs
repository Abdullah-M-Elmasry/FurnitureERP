using FurnitureERP.Domain.Entities.Products;

public class PurchaseInvoiceItem
{
    public int Id { get; set; }

    public int PurchaseInvoiceId { get; set; }

    public PurchaseInvoice PurchaseInvoice { get; set; } = null!;

    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public Product Product { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal CostPrice { get; set; }

    public decimal Total =>
        Quantity * CostPrice;
}