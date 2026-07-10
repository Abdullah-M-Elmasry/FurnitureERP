using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Suppliers.Interfaces;
using FurnitureERP.Domain.Entities.Suppliers;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using MaterialDesignThemes.Wpf;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Windows;



namespace FurnitureERP.UI.Modules.Suppliers.ViewModels;

public partial class AddSupplierViewModel
    : CrudDialogViewModel<Supplier>,
      IDialogResult<Supplier>
{
  

    private readonly ISupplierService _supplierService;


    public AddSupplierViewModel(
     ISupplierService supplierService,
     INotificationService notificationService)
     : base(notificationService)
    {
        _supplierService = supplierService;
    }

    public Supplier? DialogResult { get; private set; }

    // ==========================
    // MODE
    // ==========================

    public string Title =>
     IsEditMode
     ? "Edit Supplier"
     : "Add Supplier";

    public string ButtonText =>
        IsEditMode
        ? "Update"
        : "Create";

    // ==========================
    // FORM FIELDS
    // ==========================

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Supplier name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    private string? name;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [StringLength(20)]
    [Required(ErrorMessage = "Phone is required")]
    private string? phone;


    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Invalid email")]
    private string? email;


    [ObservableProperty]
    private string? address;

    [ObservableProperty]
    private string? nameError;

    [ObservableProperty]
    private string? phoneError;


    private bool _isInitializing;

    public async Task Initialize()
    {
        _isInitializing = true;

        // مستقبلاً:
        // await LoadGroups();
        // await LoadCities();

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
    // VALIDATION
    // ==========================
    partial void OnNameChanged(string? value)
    {
        ValidateProperty(value, nameof(Name));

        _ = ValidateNameAsync(value);
    }

    partial void OnPhoneChanged(string? value)
    {
        ValidateProperty(value, nameof(Phone));

        _ = ValidatePhoneAsync(value);
    }

    partial void OnEmailChanged(string? value)
    {
        ValidateProperty(value, nameof(Email));
    }


    private async Task ValidateNameAsync(string? value)
    {
        NameError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _supplierService.IsNameExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            NameError = "Supplier already exists.";
        }
    }


    private async Task ValidatePhoneAsync(string? value)
    {
        PhoneError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _supplierService.IsPhoneExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            PhoneError = "Phone already exists.";
        }
    }


    protected override async Task<bool> ValidateAsync()
    {
        if (!await base.ValidateAsync())
            return false;

        NameError = null;
        PhoneError = null;

        var isValid = true;

        if (await _supplierService.IsNameExists(
            Name!,
            IsEditMode ? Entity!.Id : null))
        {
            NameError = "Supplier already exists.";
            isValid = false;
        }

        if (await _supplierService.IsPhoneExists(
            Phone!,
            IsEditMode ? Entity!.Id : null))
        {
            PhoneError = "Phone already exists.";
            isValid = false;
        }

        return isValid;
    }

    // ==========================
    // SAVE
    // ==========================

    protected override async Task SaveEntity()
    {
        if (IsEditMode)
        {
            Entity!.Name = Name!;
            Entity.Phone = Phone;
            Entity.Email = Email;
            Entity.Address = Address;

            await _supplierService.Update(Entity);

            DialogResult = Entity;
        }
        else
        {
            var supplier = new Supplier
            {
                Name = Name!,
                Phone = Phone,
                Email = Email,
                Address = Address
            };

            await _supplierService.Add(supplier);

            DialogResult = supplier;
        }
    }

    protected override void LoadEntity(Supplier supplier)
    {
        Name = supplier.Name;
        Phone = supplier.Phone;
        Email = supplier.Email;
        Address = supplier.Address;
    }
}