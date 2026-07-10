using System.Collections.Specialized;
using System.Windows;

namespace FurnitureERP.UI.Common.Controls.Lookup;

public partial class ERPAdvancedLookup
{
    private void ERPAdvancedLookup_Unloaded(
        object sender,
        RoutedEventArgs e)
    {
        if (_observableCollection != null)
        {
            _observableCollection.CollectionChanged
                -= ItemsChanged;
        }
    }
}