using FurnitureERP.Application.Common.Interfaces;
//using FurnitureERP.Domain.Entities.Purchases;
using FurnitureERP.UI.Core.Commands;
using FurnitureERP.UI.Helpers;
using FurnitureERP.UI.Modules.Customers;
using FurnitureERP.UI.Modules.Customers.Views;
using FurnitureERP.UI.Modules.Dashboard;
using FurnitureERP.UI.Modules.Inventory.Views;
using FurnitureERP.UI.Modules.Products;
using FurnitureERP.UI.Modules.Products.Views;
using FurnitureERP.UI.Modules.Purchases.Views;
using FurnitureERP.UI.Modules.Reports;
using FurnitureERP.UI.Modules.Sales;
using FurnitureERP.UI.Modules.Sales.Views;
using FurnitureERP.UI.Modules.Suppliers;
using FurnitureERP.UI.Modules.Suppliers.Views;
using FurnitureERP.UI.Modules.Users;
using FurnitureERP.UI.Services;
using FurnitureERP.UI.Shell.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;
using System.Windows.Controls;
using System.Windows.Input;


namespace FurnitureERP.UI.Shell.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ISnackbarMessageQueue SnackbarMessageQueue { get; }
      = new SnackbarMessageQueue();

    private readonly IServiceProvider _serviceProvider;

    private object _currentView = null!;
    public object CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public ICommand OpenDashboardCommand { get; }
    public ICommand OpenProductsCommand { get; }

    public ICommand OpenInventoryCommand { get; }
    public ICommand OpenPurchaseInvoiceCommand { get; } // NEW
    public ICommand OpenCustomersCommand { get; }
    public ICommand OpenUsersCommand { get; }

    public ICommand OpenSalesCommand { get; }
    public ICommand OpenSuppliersCommand { get; }
    public ICommand OpenReportsCommand { get; }

    public ICommand OpenCategoriesCommand { get; }
    public ICommand OpenUnitsCommand { get; }


    public MainViewModel(IServiceProvider serviceProvider, INotificationService notificationService)
    {
        _serviceProvider = serviceProvider;
        SnackbarMessageQueue = ((NotificationService)notificationService).MessageQueue;

        OpenDashboardCommand = new RelayCommand(OpenDashboard);
        OpenProductsCommand = new RelayCommand(OpenProducts);
        OpenInventoryCommand = new RelayCommand(OpenInventory);
        OpenPurchaseInvoiceCommand = new RelayCommand(OpenPurchaseInvoice);
        OpenCustomersCommand = new RelayCommand(OpenCustomers);
        OpenSalesCommand = new RelayCommand(OpenSales);
        OpenSuppliersCommand = new RelayCommand(OpenSuppliers);
        OpenUsersCommand = new RelayCommand(OpenUsers);
        OpenReportsCommand = new RelayCommand(OpenReports);
        OpenCategoriesCommand= new RelayCommand(OpenCategories);
        OpenUnitsCommand = new RelayCommand(OpenUnits);

        OpenDashboard();
    }



    private void OpenDashboard()
    {
        CurrentView = _serviceProvider.GetRequiredService<DashboardView>();
    }
    private void OpenProducts()
    {
        CurrentView = _serviceProvider.GetRequiredService<ProductsView>();
    }

    private void OpenInventory()
    {
        CurrentView = _serviceProvider.GetRequiredService<InventoryView>();

    }
    private void OpenPurchaseInvoice() // NEW
    {
        CurrentView = _serviceProvider.GetRequiredService<PurchaseInvoicesView>();
    }
    private void OpenCustomers()
    {
        CurrentView = _serviceProvider.GetRequiredService<CustomersView>();
    }

    private void OpenSales()
    {
        CurrentView = _serviceProvider.GetRequiredService<SalesInvoicesView>();
    }

    private void OpenSuppliers()
    {
        CurrentView = _serviceProvider.GetRequiredService<SuppliersView>();
    }

    private void OpenUsers()
    {
        CurrentView = _serviceProvider.GetRequiredService<UsersView>();
    }

    private void OpenReports()
    {
        CurrentView = _serviceProvider.GetRequiredService<ReportsView>();
    }

    private void OpenCategories ()
    {
        CurrentView = _serviceProvider.GetRequiredService<CategoriesView>();
    }

    private void OpenUnits()
    {
        CurrentView = _serviceProvider.GetRequiredService<UnitsView>();
    }
}