using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Reports;

public partial class ReportsView : UserControl
{
    public ReportsView(ReportsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}