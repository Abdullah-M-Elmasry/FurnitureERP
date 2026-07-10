using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.DTOs
{
    public class PurchaseInvoicePrintItemDto
    {
        public string Code { get; set; } = "";

        public string Name { get; set; } = "";

        public decimal Quantity { get; set; }

        public decimal CostPrice { get; set; }

        public decimal Total { get; set; }
    }
}
