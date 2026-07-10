using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureERP.Domain.Entities.Products;

namespace FurnitureERP.UI.Modules.Sales.ViewModels;

public partial class SalesInvoiceItemRowVm
    : ObservableObject
{
    [ObservableProperty]
    private Product? product;

    [ObservableProperty]
    private decimal quantity = 1;

    [ObservableProperty]
    private decimal salePrice;

    public decimal Total =>
        Quantity * SalePrice;

    public string ProductCode =>
        Product?.Code ?? "";

    public string ProductName =>
        Product?.Name ?? "";

    public event Action? Completed;

    public bool IsCompleted =>
        Product != null &&
        Quantity > 0 &&
        SalePrice > 0;
    public bool HasEnoughStock =>
    Quantity <= AvailableStock;

    [ObservableProperty]
    private decimal availableStock;

    public bool IsOutOfStock =>
        AvailableStock <= 0;

    partial void OnAvailableStockChanged(decimal value)
    {
        OnPropertyChanged(nameof(HasEnoughStock));
    }

    partial void OnQuantityChanged(decimal value)
    {
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(HasEnoughStock));
    }

   

    partial void OnSalePriceChanged(decimal value)
    {
        OnPropertyChanged(nameof(Total));
        OnPropertyChanged(nameof(IsCompleted));

        if (IsCompleted)
            Completed?.Invoke();
    }

    partial void OnProductChanged(Product? value)
    {
        OnPropertyChanged(nameof(IsCompleted));

        if (IsCompleted)
            Completed?.Invoke();

        if (value == null)
            return;

        // سعر البيع الافتراضي
        SalePrice = value.SalePrice;

        OnPropertyChanged(nameof(ProductCode));
        OnPropertyChanged(nameof(ProductName));
    }
}