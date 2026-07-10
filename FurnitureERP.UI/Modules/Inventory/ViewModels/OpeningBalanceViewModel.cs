using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using FurnitureERP.UI.Modules.Products.ViewModels;
using FurnitureERP.UI.Services;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.UI.Modules.Inventory.ViewModels;

public partial class OpeningBalanceViewModel
    : CrudDialogViewModel<OpeningBalanceDto>,
      IDialogResult<OpeningBalanceDto>
{
    private readonly IInventoryService _inventoryService;
    private readonly IDialogService _dialogService;

    public OpeningBalanceViewModel(
        IInventoryService inventoryService,
        IDialogService dialogService,
        INotificationService notificationService)
        : base(notificationService)
    {
        _inventoryService = inventoryService;
        _dialogService = dialogService;
    }

    public OpeningBalanceDto? DialogResult { get; private set; }

    // ==========================
    // MODE
    // ==========================

    public string Title => "Opening Balance";

    public string ButtonText => "Save";

    // ==========================
    // FORM FIELDS
    // ==========================

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Product is required")]
    private ProductLookupDto? selectedProduct;

    [ObservableProperty]
    private decimal currentStock;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0.01,
        double.MaxValue,
        ErrorMessage = "Quantity must be greater than zero")]
    private decimal quantity;

    [ObservableProperty]
    [StringLength(500)]
    private string? notes;

    [ObservableProperty]
    private string? productError;

    // ==========================
    // LOOKUPS
    // ==========================

    [ObservableProperty]
    private List<ProductLookupDto> products = new();

    // ==========================
    // VALIDATION
    // ==========================

    partial void OnSelectedProductChanged(ProductLookupDto? value)
    {
        ValidateProperty(value, nameof(SelectedProduct));

        CurrentStock = value?.CurrentStock ?? 0;

        ProductError = null;

        if (value != null &&
            value.CurrentStock > 0)
        {
            ProductError =
                "Opening balance has already been set for this product.";
        }
    }

    partial void OnQuantityChanged(decimal value)
        => ValidateProperty(value, nameof(Quantity));

    // ==========================
    // LOAD LOOKUPS
    // ==========================

   
    public async Task Initialize()
    {
        Products =
           await _inventoryService.GetOpeningBalanceProductsLookup();
    }

    protected override async Task SaveEntity()
    {
        if (IsEditMode)
        {
            throw new NotSupportedException(
                "Opening Balance cannot be edited.");
        }

        var dto = new OpeningBalanceDto
        {
            ProductId = SelectedProduct!.Id,
            Quantity = Quantity,
            Notes = Notes
        };

        await _inventoryService.SetOpeningBalance(dto);

        DialogResult = dto;
    }
    protected override void LoadEntity(OpeningBalanceDto entity)
    {
        SelectedProduct =
            Products.FirstOrDefault(x => x.Id == entity.ProductId);

        Quantity = entity.Quantity;

        Notes = entity.Notes;
    }


    [RelayCommand]
    private async Task AddProduct(string productName)
    {

        var product  =
           _dialogService.ShowDialog<AddProductViewModel, Product>(
               vm => vm.SetInitialName(productName));

        if (product != null)
        {
           // await Initialize();
           // SelectedProduct = product;
        }
    }
    protected override async Task<bool> ValidateAsync()
    {
        if (!await base.ValidateAsync())
            return false;

        if (SelectedProduct == null)
            return false;

        if (Quantity < 0)
            return false;

        return true;
    }
}