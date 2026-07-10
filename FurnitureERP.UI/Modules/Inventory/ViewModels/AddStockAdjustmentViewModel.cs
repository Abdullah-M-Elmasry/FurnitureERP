
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Domain.Enums;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.UI.Modules.Inventory.ViewModels;

public partial class AddStockAdjustmentViewModel
    : CrudDialogViewModel<StockAdjustmentDto>,
      IDialogResult<StockAdjustmentDto>
{
    private readonly IInventoryService _inventoryService;

    public AddStockAdjustmentViewModel(
        IInventoryService inventoryService,
        INotificationService notificationService)
        : base(notificationService)
    {
        _inventoryService = inventoryService;
    }

    public StockAdjustmentDto? DialogResult { get; private set; }

    // ============================================================
    // UI
    // ============================================================

    public string Title => "Stock Adjustment";

    public string ButtonText => "Save";

    // ============================================================
    // Fields
    // ============================================================

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Product is required.")]
    private ProductLookupDto? selectedProduct;

    [ObservableProperty]
    private decimal currentStock;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [Range(0.0001, double.MaxValue,
        ErrorMessage = "Quantity must be greater than zero.")]
    private decimal quantity;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Reason is required.")]
    private StockAdjustmentReason? selectedReason;

    [ObservableProperty]
    [MaxLength(500)]
    private string? notes;

    // ============================================================
    // Error Labels
    // ============================================================

    [ObservableProperty]
    private string? quantityError;

    // ============================================================
    // Lookup
    // ============================================================

    [ObservableProperty]
    private List<ProductLookupDto> products = new();

    [ObservableProperty]
    private List<StockAdjustmentReason> reasons = new();

    // ============================================================
    // Validation
    // ============================================================

    partial void OnSelectedProductChanged(ProductLookupDto? value)
    {
        ValidateProperty(value, nameof(SelectedProduct));

        _ = LoadCurrentStockAsync();
    }

    private async Task LoadCurrentStockAsync()
    {
        if (SelectedProduct == null)
        {
            CurrentStock = 0;
            return;
        }

        CurrentStock =
            await _inventoryService.GetAvailableStock(
                SelectedProduct.Id);
    }

   

    partial void OnQuantityChanged(decimal value)
    {
        ValidateProperty(value, nameof(Quantity));
    }

    partial void OnSelectedReasonChanged(StockAdjustmentReason? value)
    {
        ValidateProperty(value, nameof(SelectedReason));
    }

    partial void OnNotesChanged(string? value)
    {
        ValidateProperty(value, nameof(Notes));
    }

    // ============================================================
    // Initialize
    // ============================================================

    public async Task Initialize()
    {
        Products = await _inventoryService.GetProductsLookup();

        Reasons =
            Enum.GetValues<StockAdjustmentReason>()
                .ToList();
    }

    // ============================================================
    // Crud Base
    // ============================================================

    protected override void LoadEntity(
        StockAdjustmentDto entity)
    {
        Quantity = entity.Quantity;

        Notes = entity.Notes;

        SelectedReason = entity.Reason;
    }

    protected override async Task<bool> ValidateAsync()
    {
        if (!await base.ValidateAsync())
            return false;

        QuantityError = null;

        if (Quantity <= 0)
        {
            QuantityError =
                "Quantity must be greater than zero.";

            return false;
        }

        return true;
    }

// ============================================================
// Save
// ============================================================

protected override async Task SaveEntity()
    {
        var dto = new StockAdjustmentDto
        {
            ProductId = SelectedProduct!.Id,
            Quantity = Quantity,
            Reason = SelectedReason!.Value,
            Notes = Notes
        };

        await _inventoryService.ApplyStockAdjustment(dto);

        DialogResult = dto;
    }

}

