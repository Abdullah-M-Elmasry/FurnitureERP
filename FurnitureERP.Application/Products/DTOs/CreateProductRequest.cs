using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Products.DTOs
{

    public class CreateProductRequest
    {

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Barcode { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }

        public int CategoryId { get; set; }

        public int UnitId { get; set; }
    }
}
