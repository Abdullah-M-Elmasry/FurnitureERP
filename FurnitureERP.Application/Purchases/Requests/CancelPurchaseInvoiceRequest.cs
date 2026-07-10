using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.Requests
{
    public class CancelPurchaseInvoiceRequest
    {
        public int InvoiceId { get; set; }

        public string Reason { get; set; } = "";
    }
}
