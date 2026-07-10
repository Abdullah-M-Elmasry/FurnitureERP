using FurnitureERP.Domain.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureERP.Persistence.Configurations
{
    public class InventoryConfiguration
        : IEntityTypeConfiguration<ProductInventory>
    {
        public void Configure(EntityTypeBuilder<ProductInventory> builder)
        {
            builder.ToTable("Inventories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CurrentQuantity)
                   .HasColumnType("decimal(18,2)");

            // كل منتج له مخزون واحد
            builder.HasIndex(x => x.ProductId)
                   .IsUnique();

            builder.HasOne(x => x.Product)
                   .WithOne(x => x.Inventory)
                   .HasForeignKey<ProductInventory>(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}