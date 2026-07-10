namespace FurnitureERP.Application.Purchases.Requests;

public class UpdatePurchaseInvoiceItemRequest
{

    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";

    public decimal Quantity { get; set; }

    public decimal CostPrice { get; set; }
}