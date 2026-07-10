using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Application.Sales.DTOs;

public class SalesInvoiceListDto
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = "";

    public DateTime Date { get; set; }

    public string CustomerName { get; set; } = "";

    public SalesInvoiceStatus Status { get; set; }

    public decimal GrandTotal { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal RemainingAmount { get; set; }
}