namespace FurnitureERP.Application.Sales.DTOs;

public class SalesInvoicePrintDto
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = "";

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public string CustomerName { get; set; } = "";

    public string? CustomerPhone { get; set; }

    public string? CustomerAddress { get; set; }

    public string? Notes { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal SubTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public List<SalesInvoicePrintItemDto> Items { get; set; }
        = new();
}