using FurnitureERP.UI.Modules.Purchases.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Modules.Purchases.Views;

public partial class PurchaseInvoicesView : UserControl
{
    private readonly PurchaseInvoicesViewModel _viewModel;

    public PurchaseInvoicesView(
        PurchaseInvoicesViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        DataContext = _viewModel;

        Loaded += PurchaseInvoicesView_Loaded;
    }

    private async void PurchaseInvoicesView_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        Loaded -= PurchaseInvoicesView_Loaded;

        await _viewModel.Load();
    }


    private async void DataGrid_MouseDoubleClick(
    object sender,
    MouseButtonEventArgs e)
    {
        if (DataContext is PurchaseInvoicesViewModel vm &&
            vm.SelectedItem != null)
        {
            await vm.OpenInvoiceCommand.ExecuteAsync(vm.SelectedItem);
        }
    }
}