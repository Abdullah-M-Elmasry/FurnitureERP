using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.Domain.Entities.Sales;

public class SalesInvoiceItem
{
    public int Id { get; set; }

    public int SalesInvoiceId { get; set; }

    public SalesInvoice SalesInvoice { get; set; } = null!;

    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public Product Product { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal SalePrice { get; set; }

    public decimal Total =>
        Quantity * SalePrice;
}