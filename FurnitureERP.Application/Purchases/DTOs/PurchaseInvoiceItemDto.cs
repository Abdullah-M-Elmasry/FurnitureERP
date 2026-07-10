using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.DTOs
{
    public class PurchaseInvoiceItemDto
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; } = "";
    
        public string ProductName { get; set; } = "";

        public decimal Quantity { get; set; }

        public decimal CostPrice { get; set; }

        public decimal Total =>
            Quantity * CostPrice;
    }
}
