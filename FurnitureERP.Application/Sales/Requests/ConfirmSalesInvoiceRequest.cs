namespace FurnitureERP.Application.Sales.Requests;

public class ConfirmSalesInvoiceRequest
{
    public int InvoiceId { get; set; }

    public string ConfirmedBy { get; set; } = "";
}