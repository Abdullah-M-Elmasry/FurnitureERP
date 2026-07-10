namespace FurnitureERP.Application.Sales.Requests;

public class CancelSalesInvoiceRequest
{
    public int InvoiceId { get; set; }

    public string Reason { get; set; } = "";
}