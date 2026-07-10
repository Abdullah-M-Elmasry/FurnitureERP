using FurnitureERP.UI.Modules.Inventory.ViewModels;
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
    /// Interaction logic for OpeningBalanceView.xaml
    /// </summary>
    public partial class OpeningBalanceView : UserControl
    {
        public OpeningBalanceView()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserControl_Loaded;

            if (DataContext is OpeningBalanceViewModel vm)
                await vm.Initialize();
        }
    }
}