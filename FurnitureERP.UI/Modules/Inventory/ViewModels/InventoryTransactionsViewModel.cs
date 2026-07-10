using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureERP.Application.Inventory.DTOs;
using FurnitureERP.Application.Inventory.Interfaces;
using FurnitureERP.UI.Common.ViewModels;

namespace FurnitureERP.UI.Modules.Inventory.ViewModels;

public partial class InventoryTransactionsViewModel
    : CrudListViewModel<InventoryTransactionDto>
{
    private readonly IInventoryService _inventoryService;

    private int _productId;

    public InventoryTransactionsViewModel(
        IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [ObservableProperty]
    private string productName = "";

    public async Task Initialize(
        int productId,
        string productName)
    {
        
        _productId = productId;
        ProductName = productName;

        await Refresh();
    }

    public override async Task Load(bool append = false)
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var result =
                await _inventoryService.GetProductTransactions(
                    _productId,
                    SearchText ?? "",
                    CurrentPage,
                    PageSize);

            if (!append)
                Items.Clear();

            foreach (var item in result.Items)
                Items.Add(item);

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
}