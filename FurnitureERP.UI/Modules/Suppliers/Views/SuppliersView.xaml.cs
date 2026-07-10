using FurnitureERP.UI.Modules.Suppliers.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Suppliers.Views;

public partial class SuppliersView : UserControl
{
    private readonly SuppliersViewModel _viewModel;

    public SuppliersView(
        SuppliersViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;

        DataContext = _viewModel;

        Loaded += SuppliersView_Loaded;
    }

    private async void SuppliersView_Loaded(object sender,RoutedEventArgs e)
    {
        Loaded -= SuppliersView_Loaded;

        await _viewModel.Load();
    }


    
}