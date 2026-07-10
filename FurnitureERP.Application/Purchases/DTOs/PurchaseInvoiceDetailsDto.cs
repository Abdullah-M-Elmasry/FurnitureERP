using FurnitureERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.DTOs
{
    public class PurchaseInvoiceDetailsDto
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = "";

        public DateTime Date { get; set; }

        public DateTime? DueDate { get; set; }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = "";
        public string? Notes { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public PurchaseInvoiceStatus Status { get; set; }

        public List<PurchaseInvoiceItemDto> Items { get; set; }
            = new();
    }
}
