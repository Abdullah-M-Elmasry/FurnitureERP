using FurnitureERP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.DTOs
{
    public class PurchaseInvoiceListDto
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = "";

        public DateTime Date { get; set; }

        public string SupplierName { get; set; } = "";

        public PurchaseInvoiceStatus Status { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
