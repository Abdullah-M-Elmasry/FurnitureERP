using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Products.DTOs.Responses;
using FurnitureERP.Application.Products.Interfaces;
using FurnitureERP.Domain.Entities.Products;
using FurnitureERP.UI.Common.ViewModels;

namespace FurnitureERP.UI.Modules.Products.ViewModels;

public partial class ProductsViewModel
    : CrudListViewModel<ProductDto>
{
    private readonly IProductService _productService;
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;

    public ProductsViewModel(
        IProductService productService,
        INotificationService notificationService,
        IDialogService dialogService)
    {
        _productService = productService;
        _notificationService = notificationService;
        _dialogService = dialogService;
    }

    // =========================
    // LOAD DATA
    // =========================

    public override async Task Load(bool append = false)
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var result =
                await _productService.GetAll(
                    SearchText ?? "",
                    CurrentPage,
                    PageSize);

            if (!append)
                Items.Clear();

            foreach (var item in result.Items)
            {
                Items.Add(item);
            }

            TotalPages =
                (int)Math.Ceiling(
                    result.TotalCount /
                    (double)PageSize);

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
    private async Task OpenAddProduct()
    {
        var saved =
            _dialogService.ShowDialog<AddProductViewModel>();

        if (saved)
            await Refresh();
    }

    // =========================
    // EDIT
    // =========================

    [RelayCommand]
    private async Task EditProduct(Product product)
    {
        if (product == null)
            return;

        var saved =
            _dialogService.ShowDialog<AddProductViewModel>(
                vm => vm.SetEntity(product));

        if (saved)
            await Refresh();
    }

    // =========================
    // DELETE
    // =========================

    [RelayCommand]
    private async Task DeleteProduct(ProductDto product)
    {
        if (product == null)
            return;

        if (!await _dialogService.Confirm(
            $"Delete {product.Name} ?",
            "Confirm Delete"))
            return;

        await _productService.Delete(product.Id);

        Items.Remove(product);

        _notificationService.ShowSuccess(
            "Product deleted successfully");
    }
}