using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Domain.Enums
{
    public enum InventoryTransactionType
    {
        OpeningBalance = 0,
        Purchase = 1,
        Sale = 2,
        PurchaseReturn = 3,
        SalesReturn = 4,
        Adjustment = 5,
        //Damage = 6,
        Transfer = 6
    }
}
