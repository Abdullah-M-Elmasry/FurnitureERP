using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Customers.Interfaces;
using FurnitureERP.Application.Customers.Services;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Application.Products.Services;
using FurnitureERP.Application.Purchases.Interfaces;
using FurnitureERP.Application.Purchases.Services;
using FurnitureERP.Application.Sales.Interfaces;
using FurnitureERP.Application.Sales.Services;
using FurnitureERP.Application.Security.Interfaces;
using FurnitureERP.Application.Security.Services;
using FurnitureERP.Application.Suppliers.Interfaces;
using FurnitureERP.Application.Suppliers.Services;
using FurnitureERP.Infrastructure.Documents.Pdf;
using FurnitureERP.Infrastructure.Identity.Services;
using FurnitureERP.Infrastructure.Inventory.Repositories;
using FurnitureERP.Infrastructure.Inventory.Services;
using FurnitureERP.Infrastructure.Persistence;
using FurnitureERP.Infrastructure.Persistence.Seed;
using FurnitureERP.Infrastructure.Products.Repositories;
//using FurnitureERP.Infrastructure.Products.Repositories;
using FurnitureERP.Infrastructure.Purchases.Repositories;
using FurnitureERP.Infrastructure.Repositories;
using FurnitureERP.Infrastructure.Sales.Repositories;
using FurnitureERP.Infrastructure.Security.Repositories;
using FurnitureERP.Infrastructure.Services;
using FurnitureERP.Infrastructure.Suppliers.Repositories;
using FurnitureERP.UI.Modules.Customers;
using FurnitureERP.UI.Modules.Customers.ViewModels;
using FurnitureERP.UI.Modules.Customers.Views;
using FurnitureERP.UI.Modules.Dashboard;
using FurnitureERP.UI.Modules.Inventory.ViewModels;
using FurnitureERP.UI.Modules.Inventory.Views;
using FurnitureERP.UI.Modules.Products;
using FurnitureERP.UI.Modules.Products.ViewModels;
using FurnitureERP.UI.Modules.Products.Views;
using FurnitureERP.UI.Modules.Purchases.ViewModels;
using FurnitureERP.UI.Modules.Purchases.Views;
using FurnitureERP.UI.Modules.Reports;
using FurnitureERP.UI.Modules.Sales;
using FurnitureERP.UI.Modules.Sales.ViewModels;
using FurnitureERP.UI.Modules.Sales.Views;
using FurnitureERP.UI.Modules.Suppliers;
using FurnitureERP.UI.Modules.Suppliers.ViewModels;
using FurnitureERP.UI.Modules.Suppliers.Views;
using FurnitureERP.UI.Modules.Users;
using FurnitureERP.UI.Security.ViewModels;
using FurnitureERP.UI.Security.Views;
using FurnitureERP.UI.Services;
using FurnitureERP.UI.Services.Interfaces;
using FurnitureERP.UI.Shell.ViewModels;
using FurnitureERP.UI.Shell.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuestPDF.Infrastructure;
using System.Windows;

//using FurnitureERP.Infrastructure.Persistence.Repositories;

namespace FurnitureERP.UI;

public partial class App :System.Windows.Application
{
    public static IHost AppHost { get; private set; } = null!;
    
    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("Settings/appsettings.json", optional: false);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString =
                    context.Configuration.GetConnectionString("DefaultConnection");

                // =========================
                // Database
                // =========================
                services.AddDbContextFactory<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<IUnitOfWork, UnitOfWork>();

                // =========================
                // Infrastructure
                // =========================
                services.AddScoped<IAuthRepository, AuthRepository>();
                services.AddScoped<IPasswordHasher, PasswordHasher>();
                services.AddSingleton<ICurrentUserService, CurrentUserService>();
                services.AddScoped<IPermissionChecker, PermissionChecker>();

                services.AddScoped<IPdfDocumentService, PdfDocumentService>();
                // =========================
                // Application
                // =========================
                services.AddScoped<IAuthService, AuthService>();
              

                // =========================
                // UI
                // =========================
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();

                services.AddScoped<LoginViewModel>();
                services.AddScoped<LoginView>();

                // services.AddSingleton<ISnackbarMessageQueue>(new SnackbarMessageQueue());
                services.AddSingleton<INotificationService, NotificationService>();

                services.AddSingleton<IDialogService,DialogService>();

                services.AddSingleton<INavigationService,NavigationService>();

                ////User controlers////////////////////////
                // Dashboard
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<DashboardView>();

                // Products
                services.AddScoped<IProductService, ProductService>();
                services.AddScoped<IProductRepository, ProductRepository>();

                services.AddScoped<ICategoryRepository, CategoryRepository>();
                services.AddScoped<ICategoryService, CategoryService>();

                services.AddScoped<IUnitRepository, UnitRepository>();
                services.AddScoped<IUnitService, UnitService>();

                services.AddScoped<ProductsView>();
                services.AddTransient<AddProductView>();
                services.AddTransient<ProductsViewModel>();
                services.AddTransient<AddProductViewModel>();

                services.AddTransient<AddCategoryViewModel>();
                services.AddTransient<AddCategoryView>();

                services.AddTransient<AddUnitViewModel>();
                services.AddTransient<AddUnitView>();

                services.AddTransient<CategoriesViewModel>();
                services.AddTransient<CategoriesView>();

                services.AddTransient<UnitsViewModel>();
                services.AddTransient<UnitsView>();

              

                // Inventory
                services.AddScoped<IInventoryService, InventoryService>();
                services.AddScoped<IInventoryRepository, InventoryRepository>();

                services.AddTransient<InventoryTransactionsViewModel>();
                services.AddTransient<InventoryTransactionsView>();
                services.AddTransient<AddStockAdjustmentView>();

                services.AddTransient<OpeningBalanceView>();

                services.AddTransient<OpeningBalanceViewModel>();

                services.AddTransient<AddStockAdjustmentViewModel>();
                services.AddTransient<InventoryViewModel>();
                services.AddTransient<InventoryView>();

              

                // Purchase
                services.AddScoped<IPurchaseInvoiceRepository,PurchaseInvoiceRepository>();
                services.AddScoped<IPurchaseInvoiceService,PurchaseInvoiceService>();

                services.AddTransient<PurchaseInvoiceEditorViewModel>();
                services.AddTransient<PurchaseInvoiceEditorView>();
                services.AddTransient<PurchaseInvoicesViewModel>();
                services.AddTransient<PurchaseInvoicesView>();


                // Sales
                services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();

                services.AddScoped<ISalesInvoiceService, SalesInvoiceService>();

                services.AddTransient<SalesInvoiceEditorViewModel>();

                services.AddTransient<SalesInvoiceEditorView>();

                services.AddTransient<SalesInvoicesViewModel>();

                services.AddTransient<SalesInvoicesView>();

                // Suppliers
                services.AddScoped<ISupplierRepository, SupplierRepository>();
                services.AddScoped<ISupplierService, SupplierService>();

                services.AddTransient<SuppliersView>();
                services.AddTransient<SuppliersViewModel>();


                services.AddTransient<AddSupplierViewModel>();
                services.AddTransient<AddSupplierView>();

                // Customers
                services.AddScoped<ICustomerRepository,CustomerRepository>();
                services.AddScoped<ICustomerService,CustomerService>();

                services.AddTransient<CustomersView>();
                services.AddTransient<CustomersViewModel>();

                services.AddTransient< AddCustomerViewModel>();
                services.AddTransient< AddCustomerView>();
                // Users
                services.AddTransient<UsersViewModel>();
                services.AddTransient<UsersView>();

                // Reports
                services.AddTransient<ReportsViewModel>();
                services.AddTransient<ReportsView>();

            })
            .Build();
    }
    
    protected override async void OnStartup(StartupEventArgs e)
    {
        







        await AppHost.StartAsync();

        // ❌ متحطش using
        var scope = AppHost.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();


        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        await SecuritySeeder.SeedAsync(db, hasher);

        // 🔹 هنا بنفتح شاشة اللوجين
        //var login = scope.ServiceProvider.GetRequiredService<LoginView>();
        //login.Show();
        QuestPDF.Settings.License = LicenseType.Community;

        var main = scope.ServiceProvider.GetRequiredService<MainWindow>();
        main.Show();

        // 🔹 إضافة: MainWindow جاهزة مع MainViewModel و Welcome Dashboard
        // بس متفتح دلوقتي، هيفتح بعد Login تلقائي لو حابب
        // مثال: لو عايز MainWindow يفتح مباشرة بدل Login:
        // var main = AppHost.Services.GetRequiredService<MainWindow>();
        // main.Show();

        base.OnStartup(e);
    }

}
