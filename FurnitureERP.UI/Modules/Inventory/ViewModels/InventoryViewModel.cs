using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.UI.Common.ViewModels;

namespace FurnitureERP.UI.Modules.Inventory.ViewModels;

public partial class InventoryViewModel
    : CrudListViewModel<InventoryItemDto>
{
    private readonly IInventoryService _inventoryService;
    private readonly IDialogService _dialogService;

    public InventoryViewModel(
        IInventoryService inventoryService,
        IDialogService dialogService)
    {
        _inventoryService = inventoryService;
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

            var result =
                await _inventoryService.GetAll(
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
    // OPEN TRANSACTIONS
    // =========================

   [RelayCommand]
private async Task  OpenTransactions(
    InventoryItemDto item)
{
    _dialogService.ShowDialog<InventoryTransactionsViewModel>(
        async vm => await vm.Initialize(
            item.ProductId,
            item.Name));
}

    [RelayCommand]
    private async Task OpeningBalance()
    {
        var result =
            _dialogService.ShowDialog<OpeningBalanceViewModel>(
        async vm => await vm.Initialize());

        if (result)
            await Refresh();
    }

    [RelayCommand]
    private void StockAdjustment()
    {
        _dialogService.ShowDialog<AddStockAdjustmentViewModel>();
    }
}