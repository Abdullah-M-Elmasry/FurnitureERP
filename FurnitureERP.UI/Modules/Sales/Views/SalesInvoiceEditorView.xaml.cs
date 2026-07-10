using FurnitureERP.UI.Modules.Purchases.ViewModels;
using FurnitureERP.UI.Modules.Sales.ViewModels;
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

namespace FurnitureERP.UI.Modules.Sales.Views
{
    /// <summary>
    /// Interaction logic for SalesInvoiceEditorView.xaml
    /// </summary>
    public partial class SalesInvoiceEditorView : UserControl
    {

        public SalesInvoiceEditorView(SalesInvoiceEditorViewModel vm)
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
