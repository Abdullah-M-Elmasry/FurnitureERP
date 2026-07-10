using FurnitureERP.UI.Modules.Purchases.ViewModels;
using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Purchases.Views;

public partial class PurchaseInvoiceEditorView : UserControl
{
    public PurchaseInvoiceEditorView(
        PurchaseInvoiceEditorViewModel vm)
    {
        InitializeComponent();

        DataContext = vm;
    }

   
}