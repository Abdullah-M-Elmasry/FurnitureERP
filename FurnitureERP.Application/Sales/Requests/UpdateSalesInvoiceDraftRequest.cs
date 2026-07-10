namespace FurnitureERP.Application.Sales.Requests;

public class UpdateSalesInvoiceDraftRequest
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public int CustomerId { get; set; }

    public string? Notes { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal PaidAmount { get; set; }

    public List<UpdateSalesInvoiceItemRequest> Items { get; set; }
        = new();
}