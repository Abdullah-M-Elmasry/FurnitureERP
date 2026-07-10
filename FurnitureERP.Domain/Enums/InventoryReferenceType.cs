using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Domain.Enums
{
    public enum InventoryReferenceType
    {
        PurchaseInvoice = 1,
        SalesInvoice = 2,
        PurchaseReturn = 3,
        SalesReturn = 4,
        Adjustment = 5,
        Transfer = 6
    }
}
