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
    /// Interaction logic for AddStockAdjustmentView.xaml
    /// </summary>
    public partial class AddStockAdjustmentView : UserControl
    {
        public AddStockAdjustmentView()
        {
            InitializeComponent();

            Loaded += UserControl_Loaded;

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserControl_Loaded;

            if (DataContext is AddStockAdjustmentViewModel vm)
                await vm.Initialize();
        }
    }
}