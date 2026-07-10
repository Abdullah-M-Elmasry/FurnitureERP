namespace FurnitureERP.Application.Sales.Requests;

public class CreateSalesInvoiceItemRequest
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public decimal Quantity { get; set; }

    public decimal SalePrice { get; set; }
}