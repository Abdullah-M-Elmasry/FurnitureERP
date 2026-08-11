using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.DTOs;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.UI.Modules.Products.ViewModels;

public partial class AddProductViewModel
    : CrudDialogViewModel<Product>,
      IDialogResult<Product>
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IUnitService _unitService;
    private readonly IDialogService _dialogService;

    public AddProductViewModel(
        IProductService productService,
        ICategoryService categoryService,
        IUnitService unitService,
        IDialogService dialogService,
        INotificationService notificationService)
        : base(notificationService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _unitService = unitService;
        _dialogService = dialogService;

    }

    public Product? DialogResult { get; private set; }

    // ==========================
    // MODE
    // ==========================


    public string Title =>
        IsEditMode ? "Edit Product" : "Add Product";

    public string ButtonText =>
        IsEditMode ? "Update" : "Create";

    // ==========================
    // FORM FIELDS
    // ==========================

    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [StringLength(50, ErrorMessage = "Barcode is too long")]
    [MaxLength(20)]
    private string? barcode;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    [StringLength(100)]
    private string? name;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0, double.MaxValue, ErrorMessage = "Cost price is invalid")]
    private decimal costPrice;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0, double.MaxValue, ErrorMessage = "Sale price is invalid")]
    private decimal salePrice;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Category is required")]
    private ProductCategory? selectedCategory;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Unit is required")]
    private Unit? selectedUnit;


    private bool _isInitializing;



    [ObservableProperty]
    private string? codeError;

    [ObservableProperty]
    private string? barcodeError;

    [ObservableProperty]
    private string? nameError;


    //[ObservableProperty]
    //private string? categorySearchText;

    //[ObservableProperty]
    //private string? unitSearchText;
    // ==========================
    // LOOKUPS
    // ==========================

    [ObservableProperty]
    private List<ProductCategory> categories = new();

    [ObservableProperty]
    private List<Unit> units = new();

    // ==========================
    // VALIDATION
    // ==========================

    partial void OnCodeChanged(string? value)
    {
        ValidateProperty(value, nameof(Code));
        if (_isInitializing)
            return;
        _ = ValidateCodeAsync(value);
    }
    partial void OnBarcodeChanged(string? value)
    {
        ValidateProperty(value, nameof(Barcode));
        if (_isInitializing)
            return;
        _ = ValidateBarcodeAsync(value);
    }

    partial void OnNameChanged(string? value)
    {
        ValidateProperty(value, nameof(Name));
        if (_isInitializing)
            return;
        _ = ValidateNameAsync(value);
    }

    partial void OnCostPriceChanged(decimal value)
        => ValidateProperty(value, nameof(CostPrice));

    partial void OnSalePriceChanged(decimal value)
        => ValidateProperty(value, nameof(SalePrice));

    partial void OnSelectedCategoryChanged(ProductCategory? value)
        => ValidateProperty(value, nameof(SelectedCategory));

    partial void OnSelectedUnitChanged(Unit? value)
        => ValidateProperty(value, nameof(SelectedUnit));





    private async Task ValidateNameAsync(string? value)
    {
        NameError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _productService.IsNameExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            NameError = "Product name already exists.";
        }
    }
    private async Task ValidateBarcodeAsync(string? value)
    {
        BarcodeError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _productService.IsBarcodeExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            BarcodeError = "Barcode already exists.";
        }
    }
    private async Task ValidateCodeAsync(string? value)
    {
        CodeError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _productService.IsCodeExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            CodeError = "Code already exists.";
        }
    }
    // ==========================
    // LOAD LOOKUPS
    // ==========================

    public async Task LoadCategories()
    {
        Categories = await _categoryService.GetLookup();
    }

    public async Task LoadUnits()
    {
        Units = await _unitService.GetLookup();
    }

    public async Task Initialize()
    {
        _isInitializing = true;

        await LoadCategories();
        await LoadUnits();

        if (IsEditMode)
        {
            SelectedCategory =
                Categories.FirstOrDefault(x => x.Id == Entity!.CategoryId);

            SelectedUnit =
                Units.FirstOrDefault(x => x.Id == Entity!.UnitId);
        }
        else
        {
            Code = await _productService.GenerateNextCode();
            Barcode = await _productService.GenerateNextBarcode();
        }

        _isInitializing = false;
    }

    public void SetInitialName(string? name)
    {
        if (IsEditMode)
            return;

        if (string.IsNullOrWhiteSpace(name))
            return;

        Name = name.Trim();
    }
    // ==========================
    // LOAD EDIT DATA
    // ==========================

    protected override void LoadEntity(Product product)
    {
        Code = product.Code;
        Barcode = product.Barcode;
        Name = product.Name;
        CostPrice = product.CostPrice;
        SalePrice = product.SalePrice;
    }

 
    protected override async Task<bool> ValidateAsync()
    {
        if (!await base.ValidateAsync())
            return false;

        CodeError = null;
        BarcodeError = null;
        NameError = null;

        var isValid = true;

        if (await _productService.IsCodeExists(
            Code!,
            IsEditMode ? Entity!.Id : null))
        {
            CodeError = "Code already exists.";
            isValid = false;
        }

        if (await _productService.IsBarcodeExists(
            Barcode!,
            IsEditMode ? Entity!.Id : null))
        {
            BarcodeError = "Barcode already exists.";
            isValid = false;
        }

        if (await _productService.IsNameExists(
            Name!,
            IsEditMode ? Entity!.Id : null))
        {
            NameError = "Product name already exists.";
            isValid = false;
        }

        return isValid;
    }
    // ==========================
    // SAVE
    // ==========================

    protected override async Task SaveEntity()
    {
        //ValidateAllProperties();

        //if (HasErrors)
        //    return;

        //if (await _productService.IsCodeExists(
        //Code!,
        //IsEditMode ? Entity!.Id : null))
        //{
        //    CodeError = "Code already exists.";
        //    return;
        //}

        //if (await _productService.IsBarcodeExists(
        //        Barcode!,
        //        IsEditMode ? Entity!.Id : null))
        //{
        //    BarcodeError = "BarCode already exists.";
        //    return;
        //}

        //if (await _productService.IsNameExists(
        //        Name!,
        //        IsEditMode ? Entity!.Id : null))
        //{
        //    nameError = "Name already exists.";
        //    return;
        //}


        if (IsEditMode)
        {
            var request = new UpdateProductRequest
            {
                Id = Entity!.Id,
                Code = Code!,
                BarCode = Barcode,
                Name = Name!,
                CostPrice = CostPrice,
                SalePrice = SalePrice,
                CategoryId = SelectedCategory!.Id,
                UnitId = SelectedUnit!.Id
            };

            var result = await _productService.Update(request);

           // DialogResult = Entity;
        }
        else
        {
            var request = new CreateProductRequest
            {
                Code = Code!,
                Barcode = Barcode,
                Name = Name!,
                CostPrice = CostPrice,
                SalePrice = SalePrice,
                CategoryId = SelectedCategory!.Id,
                UnitId = SelectedUnit!.Id
            };

            var result = await _productService.Add(request);

            //DialogResult = request;
        }
    }

    // ==========================
    // LOOKUP COMMANDS
    // ==========================

    [RelayCommand]
    private async Task AddCategory(string? text)
    {
        var category =
            _dialogService.ShowDialog<AddCategoryViewModel, ProductCategory>(
                vm => vm.SetInitialName(text));

        if (category != null)
        {
            await LoadCategories();
            SelectedCategory = category;
        }
    }

    [RelayCommand]
    private async Task AddUnit(string? text)
    {


        var unit =
            _dialogService.ShowDialog<AddUnitViewModel, Unit>(
                vm => vm.SetInitialName(text));

        if (unit != null)
        {
            await LoadUnits();
            SelectedUnit = unit;
        }
    }
}