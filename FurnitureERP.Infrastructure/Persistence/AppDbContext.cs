using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Entities.Inventories;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Domain.Entities.Sales;

//using FurnitureERP.Domain.Entities.Purchases;
using FurnitureERP.Domain.Entities.Security;
using FurnitureERP.Domain.Entities.Suppliers;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // =========================
    // SECURITY
    // =========================

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }


    // =========================
    // PRODUCTS
    // =========================

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<Unit> Units { get; set; }


    // =========================
    // INVENTORY
    // =========================

    public DbSet<ProductInventory> Inventories { get; set; }

    public DbSet<InventoryTransaction> InventoryTransactions { get; set; }


    // =========================
    // SUPPLIERS
    // =========================

    public DbSet<Supplier> Suppliers { get; set; }


    // =========================
    // CUSTOMERS
    // =========================

    public DbSet<Customer> Customers { get; set; }


    // =========================
    // PURCHASES
    // =========================

    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }

    // =========================
    // Sales
    // =========================

    public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();

    public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();
    // =========================
    // MODEL CONFIGURATION
    // =========================

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply Entity Configurations
        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

      

    }
}