namespace FurnitureERP.Application.Sales.DTOs;

public class SalesInvoicePrintItemDto
{
    public string Code { get; set; } = "";

    public string Name { get; set; } = "";

    public decimal Quantity { get; set; }

    public decimal SalePrice { get; set; }

    public decimal Total { get; set; }
}