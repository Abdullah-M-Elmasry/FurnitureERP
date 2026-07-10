using FurnitureERP.Domain.Enums;

namespace FurnitureERP.Application.Sales.DTOs;

public class SalesInvoiceDetailsDto
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = "";

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = "";

    public string? Notes { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public SalesInvoiceStatus Status { get; set; }

    public List<SalesInvoiceItemDto> Items { get; set; }
        = new();
}