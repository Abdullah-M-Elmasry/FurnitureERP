namespace FurnitureERP.Application.Sales.DTOs;

public class SalesInvoiceItemDto
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = "";

    public string ProductName { get; set; } = "";

    public decimal Quantity { get; set; }

    public decimal SalePrice { get; set; }

    public decimal Total =>
        Quantity * SalePrice;
}