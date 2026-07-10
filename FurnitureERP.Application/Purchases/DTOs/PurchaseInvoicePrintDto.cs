using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.DTOs;


public class PurchaseInvoicePrintDto
{
    
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = "";

    public DateTime Date { get; set; }

    public DateTime? DueDate { get; set; }

    public string SupplierName { get; set; } = "";

    public string? SupplierPhone { get; set; }

    public string? SupplierAddress { get; set; }

    public string? Notes { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal SubTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public List<PurchaseInvoicePrintItemDto> Items { get; set; } = [];
}