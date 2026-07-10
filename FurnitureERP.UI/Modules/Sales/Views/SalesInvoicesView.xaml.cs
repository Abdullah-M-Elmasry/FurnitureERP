using FurnitureERP.UI.Modules.Sales.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Modules.Sales.Views;

public partial class SalesInvoicesView : UserControl
{
    private readonly SalesInvoicesViewModel _viewModel;

    public SalesInvoicesView(
        SalesInvoicesViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        DataContext = _viewModel;

        Loaded += SalesInvoicesView_Loaded;
    }

    private async void SalesInvoicesView_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        Loaded -= SalesInvoicesView_Loaded;

        await _viewModel.Load();
    }

    private async void DataGrid_MouseDoubleClick(
        object sender,
        MouseButtonEventArgs e)
    {
        if (DataContext is SalesInvoicesViewModel vm &&
            vm.SelectedItem != null)
        {
            await vm.OpenInvoiceCommand.ExecuteAsync(vm.SelectedItem);
        }
    }
}