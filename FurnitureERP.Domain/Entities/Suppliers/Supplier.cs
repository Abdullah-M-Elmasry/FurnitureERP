using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureERP.Domain.Entities.Suppliers
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


