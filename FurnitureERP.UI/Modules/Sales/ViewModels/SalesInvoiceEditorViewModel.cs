using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Customers.Interfaces;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Application.Sales.DTOs;
using FurnitureERP.Application.Sales.Interfaces;
using FurnitureERP.Application.Sales.Requests;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Domain.Enums;
using FurnitureERP.UI.Modules.Customers.ViewModels;
using FurnitureERP.UI.Modules.Products.ViewModels;
using FurnitureERP.UI.Services.Interfaces;
using System.Collections.ObjectModel;

namespace FurnitureERP.UI.Modules.Sales.ViewModels;

public partial class SalesInvoiceEditorViewModel
    : ObservableObject, INavigationAware
{
    private readonly ISalesInvoiceService _service;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IInventoryService _inventoryService;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notificationService;

    public SalesInvoiceEditorViewModel(
        ISalesInvoiceService service,
        ICustomerService customerService,
        IProductService productService,
        IInventoryService inventoryService,
        IDialogService dialogService,
        INotificationService notificationService)
    {
        _service = service;
        _customerService = customerService;
        _productService = productService;
        _inventoryService = inventoryService;
        _dialogService = dialogService;
        _notificationService = notificationService;

        Items.CollectionChanged += (_, __) =>
        {
            RecalculateTotals();
        };
    }

    private void RecalculateTotals()
    {
        OnPropertyChanged(nameof(SubTotal));
        OnPropertyChanged(nameof(GrandTotal));
        OnPropertyChanged(nameof(RemainingAmount));
    }

    #region Customer

    [ObservableProperty]
    private Customer? selectedCustomer;

    [ObservableProperty]
    private List<Customer> customers = new();

    private async Task LoadCustomers()
    {
        Customers = await _customerService.GetLookup();
    }

    [RelayCommand]
    private async Task AddCustomer(string customerName)
    {
        var customer =
            _dialogService.ShowDialog<AddCustomerViewModel, Customer>(
                vm => vm.SetInitialName(customerName));

        if (customer != null)
        {
            await LoadCustomers();
            SelectedCustomer = customer;
        }
    }

    #endregion

    #region Products

    [ObservableProperty]
    private Product? selectedProduct;

    [ObservableProperty]
    private List<Product> products = new();

    private async Task LoadProducts()
    {
        Products = await _productService.GetLookup();
    }

    [RelayCommand]
    private async Task AddProduct(string productName)
    {
        var product =
            _dialogService.ShowDialog<AddProductViewModel, Product>(
                vm => vm.SetInitialName(productName));

        if (product != null)
        {
            await LoadProducts();
            SelectedProduct = product;
        }
    }

    #endregion

    #region Header

    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string invoiceNumber = "";

    [ObservableProperty]
    private DateTime date = DateTime.Today;

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private string? notes;

    [ObservableProperty]
    private decimal discount;

    [ObservableProperty]
    private decimal tax;

    [ObservableProperty]
    private decimal paidAmount;

    [ObservableProperty]
    private SalesInvoiceStatus status =
        SalesInvoiceStatus.Draft;


    
    partial void OnDiscountChanged(decimal value)
    {
        RecalculateTotals();
    }

    partial void OnTaxChanged(decimal value)
    {
        RecalculateTotals();
    }

    partial void OnPaidAmountChanged(decimal value)
    {
        RecalculateTotals();
    }

    partial void OnStatusChanged(SalesInvoiceStatus value)
    {
        OnPropertyChanged(nameof(CanEdit));
        OnPropertyChanged(nameof(ShowSaveDraft));
        OnPropertyChanged(nameof(ShowConfirm));
        OnPropertyChanged(nameof(ShowCancel));
    }

    partial void OnInvoiceNumberChanged(string value)
    {
        OnPropertyChanged(nameof(DisplayInvoiceNumber));
    }

    #endregion

    #region UI

    public ObservableCollection<SalesInvoiceItemRowVm> Items { get; }
        = new();

    public decimal SubTotal =>
        Items.Sum(x => x.Total);

    public decimal GrandTotal =>
        SubTotal + Tax - Discount;

    public decimal RemainingAmount =>
        GrandTotal - PaidAmount;

    public string DisplayInvoiceNumber =>
        string.IsNullOrWhiteSpace(InvoiceNumber)
            ? "Auto"
            : InvoiceNumber;

    private bool _isEditMode;

    public bool IsEditMode => _isEditMode;

    public bool CanEdit =>
        Status == SalesInvoiceStatus.Draft;

    public bool ShowSaveDraft =>
        Status == SalesInvoiceStatus.Draft;

    public bool ShowConfirm =>
        _isEditMode &&
        Status == SalesInvoiceStatus.Draft;

    public bool ShowCancel =>
        _isEditMode &&
        Status == SalesInvoiceStatus.Draft;

    #endregion

    private bool Validate()
    {
        if (SelectedCustomer == null)
        {
            _notificationService.ShowWarning(
                "Please select a customer.");

            return false;
        }

        if (!Items.Any(x => x.Product != null))
        {
            _notificationService.ShowWarning(
                "Please add at least one product.");

            return false;
        }

        return true;
    }

    public async Task LoadInvoice(int invoiceId)
    {
        var invoice =
            await _service.GetById(invoiceId);

        if (invoice == null)
            return;

        _isEditMode = true;

        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(ShowSaveDraft));
        OnPropertyChanged(nameof(ShowConfirm));
        OnPropertyChanged(nameof(ShowCancel));
        OnPropertyChanged(nameof(DisplayInvoiceNumber));

        Id = invoice.Id;
        InvoiceNumber = invoice.InvoiceNumber;
        Date = invoice.Date;
        DueDate = invoice.DueDate;

        Notes = invoice.Notes;

        Discount = invoice.Discount;
        Tax = invoice.Tax;
        PaidAmount = invoice.PaidAmount;

        Status = invoice.Status;

        SelectedCustomer =
            Customers.FirstOrDefault(x =>
                x.Id == invoice.CustomerId);

        Items.Clear();

        foreach (var item in invoice.Items)
        {
            var product =
                Products.FirstOrDefault(x =>
                    x.Id == item.ProductId);

            var row =
                new SalesInvoiceItemRowVm
                {
                    Product = product,
                    Quantity = item.Quantity,
                    SalePrice = item.SalePrice
                };

            if (product != null)
            {
                row.AvailableStock =
                    await _inventoryService.GetAvailableStock(product.Id);
            }

            Items.Add(row);
           
            SubscribeRow(row);

          
        }

        RecalculateTotals();
    }

    #region Save Draft

    [RelayCommand]
    private async Task SaveDraft()
    {
        if (!Validate())
            return;

        var invoiceItems =
            Items
            .Where(x => x.IsCompleted)
            .ToList();

        if (!_isEditMode)
        {
            var request =
                new CreateSalesInvoiceDraftRequest
                {
                    Date = Date,
                    DueDate = DueDate,

                    CustomerId = SelectedCustomer!.Id,

                    Notes = Notes,

                    Discount = Discount,
                    Tax = Tax,

                    PaidAmount = PaidAmount,

                    Items = invoiceItems.Select(x =>
                        new CreateSalesInvoiceItemRequest
                        {
                            ProductId = x.Product!.Id,
                            ProductName = x.Product.Name,

                            Quantity = x.Quantity,

                            SalePrice = x.SalePrice
                        })
                        .ToList()
                };

            Id = await _service.CreateDraft(request);

            _notificationService.ShowSuccess(
                "Draft invoice saved successfully.");

            await OnNavigatedTo(Id);
        }
        else
        {
            var request =
                new UpdateSalesInvoiceDraftRequest
                {
                    Id = Id,

                    Date = Date,
                    DueDate = DueDate,

                    CustomerId = SelectedCustomer!.Id,

                    Notes = Notes,

                    Discount = Discount,
                    Tax = Tax,

                    PaidAmount = PaidAmount,

                    Items = invoiceItems.Select(x =>
                        new UpdateSalesInvoiceItemRequest
                        {
                            ProductId = x.Product!.Id,
                            ProductName = x.Product.Name,

                            Quantity = x.Quantity,

                            SalePrice = x.SalePrice
                        })
                        .ToList()
                };

            await _service.UpdateDraft(request);

            _notificationService.ShowSuccess(
                "Draft invoice updated successfully.");
        }
    }

    #endregion


    #region Items

    private void SubscribeRow(SalesInvoiceItemRowVm row)
    {
        row.PropertyChanged += async (_, e) =>
        {
            if (e.PropertyName == nameof(row.Total))
            {
                RecalculateTotals();
            }

            if (e.PropertyName == nameof(row.Product))
            {
                if (row.Product != null)
                {
                    row.AvailableStock =
                        await _inventoryService.GetAvailableStock(row.Product.Id);
                }
                else
                {
                    row.AvailableStock = 0;
                }
            }
        };
    }

    private void MergeDuplicateProduct(
        SalesInvoiceItemRowVm currentRow)
    {
        if (currentRow.Product == null)
            return;

        var existingRow =
            Items.FirstOrDefault(x =>
                x != currentRow &&
                x.Product?.Id ==
                currentRow.Product.Id);

        if (existingRow == null)
            return;

        existingRow.Quantity +=
            currentRow.Quantity;

        Items.Remove(currentRow);

        _notificationService.ShowSuccess(
            $"{existingRow.Product!.Name} quantity increased.");
    }

    private void EnsureEmptyRow()
    {
        if (!Items.Any())
        {
            AddItem();
            return;
        }

        var last = Items.Last();

        if (last.IsCompleted)
        {
            AddItem();
        }
    }

    private bool CanAddNewRow()
    {
        if (!Items.Any())
            return true;

        return Items.Last().IsCompleted;
    }

    [RelayCommand]
    private void AddItem()
    {
        if (!CanAddNewRow())
        {
            _notificationService.ShowWarning(
                "Complete the current row first.");

            return;
        }

        var row =
            new SalesInvoiceItemRowVm();

        SubscribeRow(row);

        Items.Add(row);
    }

    [RelayCommand]
    private void RemoveItem(
        SalesInvoiceItemRowVm item)
    {
        if (item == null)
            return;

        Items.Remove(item);

        RecalculateTotals();
    }

    #endregion

    #region Confirm / Cancel


    private async Task<bool> ValidateStockBeforeConfirm()
    {
        var errors = new List<string>();

        foreach (var row in Items.Where(x => x.Product != null))
        {
            var available =
                await _inventoryService.GetAvailableStock(row.Product!.Id);

            if (row.Quantity > available)
            {
                errors.Add(
                    $"{row.Product.Name} (Available: {available}, Requested: {row.Quantity})");
            }
        }

        if (errors.Any())
        {
            _notificationService.ShowWarning(
                "Insufficient stock:\n\n" +
                string.Join("\n", errors));

            return false;
        }

        return true;
    }

    [RelayCommand]
    private async Task Confirm()
    {
        if (Status != SalesInvoiceStatus.Draft)
            return;

        if (!await ValidateStockBeforeConfirm())
            return;


        await _service.Confirm(
            new ConfirmSalesInvoiceRequest
            {
                InvoiceId = Id,
                ConfirmedBy = "Admin"
            });

        Status =
            SalesInvoiceStatus.Confirmed;

        _notificationService.ShowSuccess(
            "Invoice confirmed successfully.");
    }

    [RelayCommand]
    private async Task CancelInvoice()
    {
        if (Status != SalesInvoiceStatus.Draft)
            return;

        await _service.Cancel(
            new CancelSalesInvoiceRequest
            {
                InvoiceId = Id,
                Reason = "Cancelled by user"
            });

        Status =
            SalesInvoiceStatus.Cancelled;

        _notificationService.ShowSuccess(
            "Invoice cancelled.");
    }

    #endregion

    #region Navigation

    public async Task OnNavigatedTo(object? parameter)
    {
        if (Customers.Count == 0)
            await LoadCustomers();

        if (Products.Count == 0)
            await LoadProducts();

        if (parameter is int id)
            await LoadInvoice(id);
        else
            ResetInvoice();
    }

    private void ResetInvoice()
    {
        _isEditMode = false;

        Id = 0;

        InvoiceNumber = "";

        Date = DateTime.Today;

        DueDate = null;

        SelectedCustomer = null;

        Notes = null;

        Discount = 0;

        Tax = 0;

        PaidAmount = 0;

        Status = SalesInvoiceStatus.Draft;

        Items.Clear();

        var row =
            new SalesInvoiceItemRowVm();

        SubscribeRow(row);

        Items.Add(row);

        RecalculateTotals();

        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(ShowSaveDraft));
        OnPropertyChanged(nameof(ShowConfirm));
        OnPropertyChanged(nameof(ShowCancel));
        OnPropertyChanged(nameof(DisplayInvoiceNumber));
    }

    #endregion
}