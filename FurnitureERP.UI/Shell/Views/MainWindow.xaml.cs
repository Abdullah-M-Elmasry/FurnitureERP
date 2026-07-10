using FurnitureERP.UI.Shell.ViewModels;
using System.Windows;

namespace FurnitureERP.UI.Shell.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm; // ربط MainViewModel
        }
    }
}