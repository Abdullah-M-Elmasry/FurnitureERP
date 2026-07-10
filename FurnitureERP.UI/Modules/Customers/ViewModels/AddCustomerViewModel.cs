using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Customers.Interfaces;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.UI.Modules.Customers.ViewModels;

public partial class AddCustomerViewModel
    : CrudDialogViewModel<Customer>,
      IDialogResult<Customer>
{
    private readonly ICustomerService _customerService;

    public AddCustomerViewModel(
        ICustomerService customerService,
        INotificationService notificationService)
        : base(notificationService)
    {
        _customerService = customerService;
    }

    public Customer? DialogResult { get; private set; }

    // ==========================
    // MODE
    // ==========================

    public string Title =>
        IsEditMode
            ? "Edit Customer"
            : "Add Customer";

    public string ButtonText =>
        IsEditMode
            ? "Update"
            : "Create";

    // ==========================
    // FIELDS
    // ==========================

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Customer name is required")]
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
    private string? notes;

    [ObservableProperty]
    private bool isActive = true;

    // ==========================
    // CUSTOM ERRORS
    // ==========================

    [ObservableProperty]
    private string? nameError;

    [ObservableProperty]
    private string? phoneError;

    // ==========================
    // INITIALIZE
    // ==========================

    public Task Initialize()
    {
        return Task.CompletedTask;
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

        if (await _customerService.IsNameExists(
                value,
                IsEditMode ? Entity?.Id : null))
        {
            NameError = "Customer already exists.";
        }
    }

    private async Task ValidatePhoneAsync(string? value)
    {
        PhoneError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _customerService.IsPhoneExists(
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

        if (await _customerService.IsNameExists(
                Name!,
                IsEditMode ? Entity!.Id : null))
        {
            NameError = "Customer already exists.";
            isValid = false;
        }

        if (await _customerService.IsPhoneExists(
                Phone!,
                IsEditMode ? Entity!.Id : null))
        {
            PhoneError = "Phone already exists.";
            isValid = false;
        }

        return isValid;
    }

    // ==========================
    // LOAD ENTITY
    // ==========================

    protected override void LoadEntity(Customer customer)
    {
        Name = customer.Name;
        Phone = customer.Phone;
        Email = customer.Email;
        Address = customer.Address;
        Notes = customer.Notes;
        IsActive = customer.IsActive;
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
            Entity.Notes = Notes;
            Entity.IsActive = IsActive;

            await _customerService.Update(Entity);

            DialogResult = Entity;
        }
        else
        {
            var customer = new Customer
            {
                Name = Name!,
                Phone = Phone,
                Email = Email,
                Address = Address,
                Notes = Notes,
                IsActive = IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _customerService.Add(customer);

            DialogResult = customer;
        }
    }
}