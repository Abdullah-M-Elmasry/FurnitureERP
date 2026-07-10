using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Purchases.Requests
{
    public class ConfirmPurchaseInvoiceRequest
    {
        public int InvoiceId { get; set; }

        public string ConfirmedBy { get; set; } = "";
    }
}
