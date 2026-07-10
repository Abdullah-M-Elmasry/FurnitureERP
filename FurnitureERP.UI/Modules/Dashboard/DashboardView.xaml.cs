using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Dashboard;

public partial class DashboardView : UserControl
{
    public DashboardView(DashboardViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}