
using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Suppliers.Interfaces;
using FurnitureERP.Domain.Entities.Suppliers;
using FurnitureERP.UI.Common.ViewModels;
using System.Collections.ObjectModel;


namespace FurnitureERP.UI.Modules.Suppliers.ViewModels;

public partial class SuppliersViewModel : CrudListViewModel<Supplier>
{
    private readonly ISupplierService _supplierService;
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;

    public SuppliersViewModel(
        ISupplierService supplierService,
        INotificationService notificationService,
        IDialogService dialogService)
    {
        _supplierService = supplierService;
        _notificationService = notificationService;
        _dialogService = dialogService;
    }


    // =========================
    // LOAD DATA
    // =========================
    public override async Task Load(
    bool append = false)
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var result =
                await _supplierService.GetAll(
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
    private async Task OpenAddSupplier()
    {
        var saved = _dialogService.ShowDialog<AddSupplierViewModel>();

        if (saved)
            await Refresh();
    }

    // =========================
    // EDIT
    // =========================
    [RelayCommand]
    private async Task EditSupplier(Supplier supplier)
    {
        if (supplier == null)
            return;

        var saved = _dialogService.ShowDialog<AddSupplierViewModel>(
           vm => vm.SetEntity(supplier));

        if (saved)
            await Refresh();
    }

    // =========================
    // DELETE
    // =========================
    [RelayCommand]
    private async Task DeleteSupplier(Supplier supplier)
    {
        if (supplier == null)
            return;

        if (!await _dialogService.Confirm(
            $"Delete {supplier.Name} ?",
            "Confirm Delete"))
            return;

        await _supplierService.Delete(supplier.Id);

        Items.Remove(supplier);

        _notificationService.ShowSuccess(
            "Supplier deleted successfully");
    }


}