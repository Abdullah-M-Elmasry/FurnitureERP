using FurnitureERP.UI.Modules.Inventory.ViewModels;
using FurnitureERP.UI.Modules.Products.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FurnitureERP.UI.Modules.Inventory.Views
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : UserControl
    {
        private readonly InventoryViewModel _viewModel;

        public InventoryView(
            InventoryViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;

            Loaded += InventoryView_Loaded;
        }

        private async void InventoryView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= InventoryView_Loaded;

            await _viewModel.Load();
        }
    }
}
