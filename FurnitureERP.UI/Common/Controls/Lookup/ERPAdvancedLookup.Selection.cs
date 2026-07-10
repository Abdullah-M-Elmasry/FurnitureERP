using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls.Lookup;

public partial class ERPAdvancedLookup
{
    private static void OnSelectedChanged(
   DependencyObject d,
   DependencyPropertyChangedEventArgs e)
    {
        var control = (ERPAdvancedLookup)d;

        if (control._updatingSelection)
            return;

        if (e.NewValue != null)
            control.UpdateSelection(e.NewValue);

       // ((ERPAdvancedLookup)d).SelectionCommitted?.Invoke(d, EventArgs.Empty);
    }



    private void UpdateSelection(object item)
    {
        _updatingSelection = true;

        try
        {
           // SelectedItem = item;   // <-- الناقص

            SearchText =
                item.GetType()
                    .GetProperty(DisplayMemberPath)?
                    .GetValue(item)?
                    .ToString()
                ?? "";

            IsPopupOpen = false;
        }
        finally
        {
            _updatingSelection = false;
        }
    }

    //private void PART_List_SelectionChanged(
    // object sender,
    // SelectionChangedEventArgs e)
    //{
    //    if (PART_List.SelectedItem != null)
    //        SelectedItem = PART_List.SelectedItem;

    //    //if (PART_List.SelectedItem == null)
    //    //    return;

    //    //SelectedItem = PART_List.SelectedItem;

    //    //MessageBox.Show(
    //    //    $"SelectedItem = {SelectedItem}");
    //}

    private void PART_List_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
    {
        if (PART_List.SelectedItem == null)
            return;

        SelectedItem = PART_List.SelectedItem;

        IsPopupOpen = false;

        Dispatcher.BeginInvoke(() =>
        {
            MoveFocus(
                new TraversalRequest(
                    FocusNavigationDirection.Next));
        });
    }

}