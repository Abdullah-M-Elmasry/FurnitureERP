using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using System.ComponentModel.DataAnnotations;

namespace FurnitureERP.UI.Modules.Products.ViewModels;

public partial class AddUnitViewModel
    : CrudDialogViewModel<Unit>,
      IDialogResult<Unit>
{
    private readonly IUnitService _unitService;

    public AddUnitViewModel(
        IUnitService unitService,
        INotificationService notificationService)
        : base(notificationService)
    {
        _unitService = unitService;
    }

    public string Title =>
        IsEditMode ? "Edit Unit" : "Add Unit";

    public string ButtonText =>
        IsEditMode ? "Update" : "Create";

    public Unit? DialogResult { get; private set; }

    // ==========================
    // FORM
    // ==========================

    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [StringLength(100)]
    private string? name;

    [ObservableProperty]
    private string? codeError;

    [ObservableProperty]
    private string? nameError;

    private bool _isInitializing;

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

    partial void OnNameChanged(string? value)
    {
        ValidateProperty(value, nameof(Name));

        if (_isInitializing)
            return;

        _ = ValidateNameAsync(value);
    }

    private async Task ValidateCodeAsync(string? value)
    {
        CodeError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _unitService.IsCodeExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            CodeError = "Code already exists.";
        }
    }

    private async Task ValidateNameAsync(string? value)
    {
        NameError = null;

        if (string.IsNullOrWhiteSpace(value))
            return;

        if (await _unitService.IsNameExists(
            value,
            IsEditMode ? Entity?.Id : null))
        {
            NameError = "Unit already exists.";
        }
    }

    // ==========================
    // INITIALIZE
    // ==========================

    public async Task Initialize()
    {
        _isInitializing = true;

        if (!IsEditMode)
        {
            Code = await _unitService.GenerateNextCode();
        }

        _isInitializing = false;
    }

    protected override void LoadEntity(Unit entity)
    {
        Code = entity.Code;
        Name = entity.Name;
    }

    // ==========================
    // VALIDATE BEFORE SAVE
    // ==========================

    protected override async Task<bool> ValidateAsync()
    {
        if (!await base.ValidateAsync())
            return false;

        CodeError = null;
        NameError = null;

        var isValid = true;

        if (await _unitService.IsCodeExists(
            Code!,
            IsEditMode ? Entity!.Id : null))
        {
            CodeError = "Code already exists.";
            isValid = false;
        }

        if (await _unitService.IsNameExists(
            Name!,
            IsEditMode ? Entity!.Id : null))
        {
            NameError = "Unit already exists.";
            isValid = false;
        }

        return isValid;
    }

    // ==========================
    // INITIAL NAME
    // ==========================

    public void SetInitialName(string? name)
    {
        if (IsEditMode)
            return;

        if (string.IsNullOrWhiteSpace(name))
            return;

        Name = name.Trim();
    }

    // ==========================
    // SAVE
    // ==========================

    protected override async Task SaveEntity()
    {
        if (IsEditMode)
        {
            Entity!.Code = Code!;
            Entity.Name = Name!;

            await _unitService.Update(Entity);

            DialogResult = Entity;
        }
        else
        {
            var unit = new Unit
            {
                Code = Code!,
                Name = Name!,
                IsActive = true
            };

            await _unitService.Add(unit);

            DialogResult = unit;
        }
    }
}