using FurnitureERP.UI.Modules.Products.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Products.Views;

public partial class AddUnitView : UserControl
{
    public AddUnitView()
    {
        InitializeComponent();

        Loaded += UserControl_Loaded;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= UserControl_Loaded;

        if (DataContext is AddUnitViewModel vm)
            await vm.Initialize();
    }
}