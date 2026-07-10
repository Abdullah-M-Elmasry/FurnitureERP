using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Application.Products.DTOs
{
    public class ProductLookupDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal CurrentStock { get; set; }

        public override string ToString()
            => $"{Code} - {Name}";
    }
}
