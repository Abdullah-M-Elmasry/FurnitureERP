using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Products.DTOs.Responses
{

    public class ProductDetailsDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string BarCode { get; set; }= string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal SalePrice { get; set; }

        public decimal CostPrice { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int UnitId { get; set; }

        public string UnitName { get; set; } = string.Empty;
    }
}
