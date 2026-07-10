using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FurnitureERP.UI.Common.Collections;

public class ObservableRangeCollection<T>
    : ObservableCollection<T>
{
    public void ReplaceRange(
        IEnumerable<T> items)
    {
        Items.Clear();

        foreach (var item in items)
            Items.Add(item);

        OnCollectionChanged(
            new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset));
    }

    public void AddRange(
        IEnumerable<T> items)
    {
        foreach (var item in items)
            Items.Add(item);

        OnCollectionChanged(
            new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Reset));
    }
}