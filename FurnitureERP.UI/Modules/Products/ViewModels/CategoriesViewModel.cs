using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.UI.Common.ViewModels;

namespace FurnitureERP.UI.Modules.Products.ViewModels;

public partial class CategoriesViewModel
    : CrudListViewModel<ProductCategory>
{
    private readonly ICategoryService _categoryService;
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;

    public CategoriesViewModel(
        ICategoryService categoryService,
        INotificationService notificationService,
        IDialogService dialogService)
    {
        _categoryService = categoryService;
        _notificationService = notificationService;
        _dialogService = dialogService;
    }

    // =========================
    // LOAD
    // =========================

    public override async Task Load(bool append = false)
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var result = await _categoryService.GetAll(
                SearchText ?? "",
                CurrentPage,
                PageSize);

            if (!append)
                Items.Clear();

            foreach (var item in result.Items)
                Items.Add(item);

            TotalPages =
                (int)Math.Ceiling(result.TotalCount / (double)PageSize);

            HasMoreItems =
                CurrentPage < TotalPages;
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================
    // ADD
    // =========================

    [RelayCommand]
    private async Task OpenAddCategory()
    {
        var saved =
            _dialogService.ShowDialog<AddCategoryViewModel>();

        if (saved)
            await Refresh();
    }

    // =========================
    // EDIT
    // =========================

    [RelayCommand]
    private async Task EditCategory(ProductCategory category)
    {
        if (category == null)
            return;

        var saved =
            _dialogService.ShowDialog<AddCategoryViewModel>(
                vm => vm.SetEntity(category));

        if (saved)
            await Refresh();
    }

    // =========================
    // DELETE
    // =========================

    [RelayCommand]
    private async Task DeleteCategory(ProductCategory category)
    {
        if (category == null)
            return;

        if (!await _dialogService.Confirm(
            $"Delete {category.Name} ?",
            "Confirm Delete"))
            return;

        await _categoryService.Delete(category.Id);

        Items.Remove(category);

        _notificationService.ShowSuccess(
            "Category deleted successfully.");
    }
}