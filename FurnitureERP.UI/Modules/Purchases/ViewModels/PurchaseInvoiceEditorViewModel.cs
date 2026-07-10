using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Application.Products.Services;
using FurnitureERP.Application.Purchases.DTOs;
using FurnitureERP.Application.Purchases.Interfaces;
using FurnitureERP.Application.Purchases.Requests;
using FurnitureERP.Application.Suppliers.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.Domain.Entities.Suppliers;
using FurnitureERP.Domain.Enums;
using FurnitureERP.UI.Modules.Products.ViewModels;
using FurnitureERP.UI.Modules.Suppliers.ViewModels;
using FurnitureERP.UI.Services.Interfaces;
using System.Collections.ObjectModel;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;
using static System.Net.Mime.MediaTypeNames;




namespace FurnitureERP.UI.Modules.Purchases.ViewModels;

public partial class PurchaseInvoiceEditorViewModel
    : ObservableObject, INavigationAware
{
    private readonly IPurchaseInvoiceService _service;
    private readonly ISupplierService _supplierService;
    private readonly IProductService _productService;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notificationService;
    public PurchaseInvoiceEditorViewModel(
      IPurchaseInvoiceService service,
      ISupplierService supplierService,
      IProductService productService,
      IDialogService dialogService,
      INotificationService notificationService)
    {
        _service = service;
        _supplierService = supplierService;
        _productService = productService;
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
    }

    [ObservableProperty]
    private Supplier? selectedSupplier;


    [ObservableProperty]
    private List<Supplier> suppliers = new();


    private async Task LoadSuppliers()
    {
        Suppliers = await _supplierService.GetLookup();
    }

    [RelayCommand]
    private async Task AddSupplier(string supplierName)
    {
        var supplier =
           _dialogService.ShowDialog<AddSupplierViewModel, Supplier>(
               vm => vm.SetInitialName(supplierName));

        if (supplier != null)
        {
            await LoadSuppliers();
            SelectedSupplier = supplier;
        }
    }



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

    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string invoiceNumber = "";

    [ObservableProperty]
    private DateTime date = DateTime.Today;

    [ObservableProperty]
    private DateTime? dueDate;

    [ObservableProperty]
    private int supplierId;

    [ObservableProperty]
    private string supplierName = "";

    [ObservableProperty]
    private string? notes;

    [ObservableProperty]
    private decimal discount;

    [ObservableProperty]
    private decimal tax;

   

    [ObservableProperty]
    private PurchaseInvoiceStatus status =
        PurchaseInvoiceStatus.Draft;

    public bool CanEdit =>
    Status == PurchaseInvoiceStatus.Draft;

    public bool ShowSaveDraft =>
      Status == PurchaseInvoiceStatus.Draft;

    public bool ShowConfirm =>
    _isEditMode &&
    Status == PurchaseInvoiceStatus.Draft;

    public bool ShowCancel =>
    _isEditMode &&
    Status == PurchaseInvoiceStatus.Draft;

    partial void OnStatusChanged(PurchaseInvoiceStatus value)
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

    public ObservableCollection<PurchaseInvoiceItemRowVm>
        Items
    { get; } = new();

    public decimal SubTotal =>
        Items.Sum(x => x.Total);

    public decimal GrandTotal =>
        SubTotal + Tax - Discount;


    public string DisplayInvoiceNumber =>
    string.IsNullOrWhiteSpace(InvoiceNumber)
        ? "Auto"
        : InvoiceNumber;

    private bool _isEditMode;
    public bool IsEditMode => _isEditMode;

    //private bool _isInitialized;

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

        Status = invoice.Status;

        SelectedSupplier =
    Suppliers.FirstOrDefault(x =>
        x.Id == invoice.SupplierId);

        Items.Clear();
        RecalculateTotals();
        foreach (var item in invoice.Items)
        {
            var product =
                Products.FirstOrDefault(x =>
                    x.Id == item.ProductId);

            var row =
                new PurchaseInvoiceItemRowVm
                {
                   
                    Product = product,
                    
                    Quantity = item.Quantity,
                    CostPrice = item.CostPrice
                };

            SubscribeRow(row);

            Items.Add(row);
        }

        RecalculateTotals();

       
    }

    //partial void OnStatusChanged(PurchaseInvoiceStatus value)
    //{
    //    OnPropertyChanged(nameof(CanEdit));
    //}
    partial void OnDiscountChanged(decimal value)
    {
        OnPropertyChanged(nameof(GrandTotal));
    }

    partial void OnTaxChanged(decimal value)
    {
        OnPropertyChanged(nameof(GrandTotal));
    }


    private bool Validate()
    {
        if (SelectedSupplier == null)
        {
            _notificationService.ShowWarning("Please select a supplier.");
            return false;
        }

        if (!Items.Any(x => x.Product != null))
        {
            _notificationService.ShowWarning("Please add at least one product.");
            return false;
        }

        return true;
    }



    [RelayCommand]
    private async Task SaveDraft()
    {
        if (!Validate())
            return;


        if (!_isEditMode)
        {
          
            var invoiceItems =
    Items
    .Where(x => x.IsCompleted)
    .ToList();

            var request =
                new CreatePurchaseInvoiceDraftRequest
                {
                    Date = Date,
                    DueDate = DueDate,
                    SupplierId = SelectedSupplier.Id,
                    Notes = Notes,
                    Discount = Discount,
                    Tax = Tax,


                    Items = invoiceItems.Select(x =>

                        new CreatePurchaseInvoiceItemRequest
                        {
                             

                            ProductId = x.Product!.Id,
                            ProductName = x.Product!.Name,
                            Quantity = x.Quantity,
                            CostPrice = x.CostPrice
                        })
                        .ToList()
                };

            Id =
                await _service.CreateDraft(request);
            //  OnPropertyChanged(nameof(CanConfirm));
            _notificationService.ShowSuccess(
    "Draft invoice saved successfully.");

            await OnNavigatedTo(Id);

        }
        else
        {

            var invoiceItems =
    Items
    .Where(x => x.IsCompleted)
    .ToList();
            var request =
                new UpdatePurchaseInvoiceDraftRequest
                {
                    Id = Id,
                    Date = Date,
                    DueDate = DueDate,
                    SupplierId = SelectedSupplier.Id,
                    Notes = Notes,
                    Discount = Discount,
                    Tax = Tax,

                    Items = invoiceItems.Select(x =>
                        new UpdatePurchaseInvoiceItemRequest
                        {
                            ProductId = x.Product!.Id,
                            ProductName = x.Product!.Name,
                            Quantity = x.Quantity,
                            CostPrice = x.CostPrice
                        })
                        .ToList()
                };

            await _service.UpdateDraft(request);

            _notificationService.ShowSuccess(
    "Draft invoice updated successfully.");
        }
    }

    private void SubscribeRow(PurchaseInvoiceItemRowVm row)
    {
        row.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(row.Total))
                RecalculateTotals();
        };

        row.Completed += () =>
        {
            MergeDuplicateProduct(row);

            EnsureEmptyRow();
        };
    }

    private void MergeDuplicateProduct(PurchaseInvoiceItemRowVm currentRow)
    {
        if (currentRow.Product == null)
            return;

        var existingRow = Items.FirstOrDefault(x =>
            x != currentRow &&
            x.Product?.Id == currentRow.Product.Id);

        if (existingRow == null)
            return;

        existingRow.Quantity += currentRow.Quantity;

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

        var item = new PurchaseInvoiceItemRowVm();

        SubscribeRow(item);

        Items.Add(item);
    }

    [RelayCommand]
    private void RemoveItem(
     PurchaseInvoiceItemRowVm item)
    {
        if (item == null)
            return;

        Items.Remove(item);

        RecalculateTotals();
    }


    [RelayCommand]
    private async Task Confirm()
    {
        if (Status != PurchaseInvoiceStatus.Draft)
            return;
      

        await _service.Confirm(
            new ConfirmPurchaseInvoiceRequest
            {
                InvoiceId = Id,
                ConfirmedBy = "Admin"
            });

        Status =
            PurchaseInvoiceStatus.Confirmed;

        _notificationService.ShowSuccess(
            "Invoice confirmed successfully");
    }

    [RelayCommand]
    private async Task CancelInvoice()
    {
        if (Status != PurchaseInvoiceStatus.Draft)
            return;

        await _service.Cancel(
            new CancelPurchaseInvoiceRequest
            {
                InvoiceId = Id,
                Reason = "Cancelled by user"
            });

        Status =
            PurchaseInvoiceStatus.Cancelled;

        _notificationService.ShowSuccess(
            "Invoice cancelled");
    }

    public async Task OnNavigatedTo(object? parameter)
    {
        if (Suppliers.Count == 0)
            await LoadSuppliers();

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

        SelectedSupplier = null;

        Notes = null;

        Discount = 0;
        Tax = 0;

        Status = PurchaseInvoiceStatus.Draft;

        Items.Clear();

        var row = new PurchaseInvoiceItemRowVm();

        SubscribeRow(row);

        Items.Add(row);

        RecalculateTotals();

        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(ShowSaveDraft));
        OnPropertyChanged(nameof(ShowConfirm));
        OnPropertyChanged(nameof(ShowCancel));
        OnPropertyChanged(nameof(DisplayInvoiceNumber));
    }
}