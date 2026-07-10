using FurnitureERP.Domain.Entities.Products;


namespace FurnitureERP.Domain.Entities.Inventories
{
    public class ProductInventory
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal CurrentQuantity { get; set; } // الرصيد الحالي

        //public int WarehouseId { get; set; } = 1;

        public DateTime UpdatedAt { get; set; } =  DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // المستخدم الذي أنشأ السجل
        public string? CreatedBy { get; set; }

        // آخر مستخدم عدل الرصيد
        public string? UpdatedBy { get; set; }

        public Product Product { get; set; } = null!;



        //public string StockStatus
        //{
        //    get
        //    {
        //        if (CurrentQuantity == 0)
        //            return "Out Of Stock";

        //        if (CurrentQuantity <= 5)
        //            return "Low Stock";

        //        return "Available";
        //    }
        //}

    }
}

