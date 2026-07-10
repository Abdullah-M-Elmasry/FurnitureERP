using FurnitureERP.UI.Modules.Customers.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Customers.Views;

public partial class CustomersView : UserControl
{
    private readonly CustomersViewModel _viewModel;

    public CustomersView(CustomersViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        DataContext = _viewModel;

        Loaded += CustomersView_Loaded;
    }

    private async void CustomersView_Loaded(
     object sender,
     RoutedEventArgs e)
    {
        Loaded -= CustomersView_Loaded;

        await _viewModel.Load();
    }
}