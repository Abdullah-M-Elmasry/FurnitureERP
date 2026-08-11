using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Products.DTOs
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string BarCode { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }

        public int CategoryId { get; set; }

        public int UnitId { get; set; }
    }
}
