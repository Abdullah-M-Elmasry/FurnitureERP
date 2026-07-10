using System.Collections;
using System.Collections.Specialized;
using System.Windows;

namespace FurnitureERP.UI.Common.Controls.Lookup;

public partial class ERPAdvancedLookup
{
    private static void OnItemsChanged(
     DependencyObject d,
     DependencyPropertyChangedEventArgs e)
    {
        var control = (ERPAdvancedLookup)d;

        if (control._observableCollection != null)
        {
            control._observableCollection.CollectionChanged
                -= control.ItemsChanged;
        }

        control._observableCollection =
            e.NewValue as INotifyCollectionChanged;

        if (control._observableCollection != null)
        {
            control._observableCollection.CollectionChanged
                += control.ItemsChanged;
        }

        control.ApplyFilter();
    }
    private void ItemsChanged(
    object? sender,
    NotifyCollectionChangedEventArgs e)
    {
        ApplyFilter();
    }


    private void ApplyFilter()
    {
        FilteredItems.ReplaceRange(

            LookupSearchEngine.Filter(
                ItemsSource,
                SearchText,
                SearchMemberPaths)

        );

        ShowAddButton =
            !string.IsNullOrWhiteSpace(SearchText)
            && FilteredItems.Count == 0;
    }

}