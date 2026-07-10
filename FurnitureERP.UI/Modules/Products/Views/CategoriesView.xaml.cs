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

namespace FurnitureERP.UI.Modules.Products.Views
{
    /// <summary>
    /// Interaction logic for CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : UserControl
    {
        private readonly CategoriesViewModel _viewModel;

        public CategoriesView(
      CategoriesViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = _viewModel;

            Loaded += CategoriesView_Loaded;
        }

        private async void CategoriesView_Loaded(
        object sender,
        RoutedEventArgs e)
        {
            Loaded -= CategoriesView_Loaded;

            await _viewModel.Load();
        }
    }
}
