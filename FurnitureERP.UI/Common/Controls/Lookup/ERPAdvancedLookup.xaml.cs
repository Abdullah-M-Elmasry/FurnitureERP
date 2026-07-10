using FurnitureERP.UI.Common.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace FurnitureERP.UI.Common.Controls.Lookup;

public partial class ERPAdvancedLookup : UserControl
{
    public ERPAdvancedLookup()
    {
        InitializeComponent();

        FilteredItems = new ObservableRangeCollection<object>();

        Unloaded += ERPAdvancedLookup_Unloaded;
    }

  //  public event EventHandler? SelectionCommitted;

    // shared fields
    private INotifyCollectionChanged? _observableCollection;

    private bool _updatingSelection;
}