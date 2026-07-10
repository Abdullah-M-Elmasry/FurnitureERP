using CommunityToolkit.Mvvm.ComponentModel;
using FurnitureERP.Domain.Entities.Products;
namespace FurnitureERP.UI.Modules.Purchases.ViewModels;

public partial class PurchaseInvoiceItemRowVm
    : ObservableObject
{
    //[ObservableProperty]
    //private int productId;

    //[ObservableProperty]
    //private string productCode = "";

    //[ObservableProperty]
    //private string productName = "";



    [ObservableProperty]
    private Product? product;

    [ObservableProperty]
    private decimal quantity = 1;

    [ObservableProperty]
    private decimal costPrice;

    public decimal Total =>
        Quantity * CostPrice;


    public string ProductCode =>
    Product?.Code ?? "";

    public string ProductName =>
        Product?.Name ?? "";


    public event Action? Completed;
    public bool IsCompleted =>
    Product != null &&
    Quantity > 0 &&
    CostPrice > 0;


    partial void OnQuantityChanged(decimal value)
    {
        OnPropertyChanged(nameof(Total));
    }

    partial void OnCostPriceChanged(decimal value)
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

        CostPrice = value.CostPrice;

        OnPropertyChanged(nameof(ProductCode));
        OnPropertyChanged(nameof(ProductName));
    }
}