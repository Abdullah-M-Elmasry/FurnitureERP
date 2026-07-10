using FurnitureERP.Domain.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureERP.Persistence.Configurations
{
    public class InventoryTransactionConfiguration
        : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.ToTable("InventoryTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ReferenceType)
           .HasMaxLength(50);

            builder.HasOne(x => x.Product)
                   .WithMany(x => x.InventoryTransactions)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}