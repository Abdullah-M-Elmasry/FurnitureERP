using FurnitureERP.UI.Modules.Products.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Products.Views;

public partial class ProductsView : UserControl
{
    private readonly ProductsViewModel _viewModel;

    public ProductsView(
        ProductsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        DataContext = _viewModel;

        Loaded += ProductsView_Loaded;
    }

    private async void ProductsView_Loaded(object sender,RoutedEventArgs e)
    {
        Loaded -= ProductsView_Loaded;

        await _viewModel.Load();
    }
}