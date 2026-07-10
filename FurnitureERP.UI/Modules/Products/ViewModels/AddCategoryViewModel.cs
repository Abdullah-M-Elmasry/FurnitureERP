using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.UI.Common.Crud;
using FurnitureERP.UI.Common.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureERP.UI.Modules.Products.ViewModels
{
    public partial class AddCategoryViewModel
    : CrudDialogViewModel<ProductCategory>,
      IDialogResult<ProductCategory>
    {

        private readonly ICategoryService _categoryService;

        public AddCategoryViewModel(
            ICategoryService categoryService,
            INotificationService notificationService)
            : base(notificationService)
        {
            _categoryService = categoryService;
        }

        public string Title =>
        IsEditMode ? "Edit Category" : "Add Category";

        public string ButtonText =>
            IsEditMode ? "Update" : "Create";

        public ProductCategory? DialogResult { get; private set; }


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

            if (await _categoryService.IsCodeExists(
                value,
                IsEditMode ? Entity?.Id : null))
            {
                CodeError = "Code already exists.";
            }
        }

        public async Task Initialize()
        {
            _isInitializing = true;

            if (!IsEditMode)
            {
                Code = await _categoryService.GenerateNextCode();
            }

            _isInitializing = false;
        }

        protected override void LoadEntity(ProductCategory entity)
        {
            Code = entity.Code;
            Name = entity.Name;
        }

        protected override async Task<bool> ValidateAsync()
        {
            if (!await base.ValidateAsync())
                return false;

            CodeError = null;
            NameError = null;

            var isValid = true;

            if (await _categoryService.IsCodeExists(
                Code!,
                IsEditMode ? Entity!.Id : null))
            {
                CodeError = "Code already exists.";
                isValid = false;
            }

            if (await _categoryService.IsNameExists(
                Name!,
                IsEditMode ? Entity!.Id : null))
            {
                NameError = "Category already exists.";
                isValid = false;
            }

            return isValid;
        }
        private async Task ValidateNameAsync(string? value)
        {
            NameError = null;

            if (string.IsNullOrWhiteSpace(value))
                return;

            if (await _categoryService.IsNameExists(
                value,
                IsEditMode ? Entity?.Id : null))
            {
                NameError = "Category already exists.";
            }
        }

        public void SetInitialName(string? name)
        {
            if (IsEditMode)
                return;

            if (string.IsNullOrWhiteSpace(name))
                return;

            Name = name.Trim();
        }
        protected override async Task SaveEntity()
        {
            if (IsEditMode)
            {
                Entity!.Code = Code!;
                Entity.Name = Name!;

                await _categoryService.Update(Entity);

                DialogResult = Entity;
            }
            else
            {
                var category = new ProductCategory
                {
                    Code = Code!,
                    Name = Name!,
                    IsActive = true
                };

                await _categoryService.Add(category);

                DialogResult = category;
            }
        }
    }
}
