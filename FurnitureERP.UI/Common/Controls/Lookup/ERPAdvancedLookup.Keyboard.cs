using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls.Lookup;

public partial class ERPAdvancedLookup
{
    private void OnKeyDown(object sender, KeyEventArgs e)
   {
        switch (e.Key)
        {
            case Key.Down:

                if (!IsPopupOpen)
                {
                    ApplyFilter();
                    IsPopupOpen = true;
                }

                if (PART_List.Items.Count > 0)
                {
                    PART_List.Focus();

                    if (PART_List.SelectedIndex < 0)
                        PART_List.SelectedIndex = 0;
                }

                e.Handled = true;
                break;

            case Key.Escape:

                IsPopupOpen = false;
                e.Handled = true;
                break;
        }
    }

    private void PART_List_PreviewKeyDown(
         object sender,
         KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:

                if (PART_List.SelectedItem != null)
                {
                    SelectedItem = PART_List.SelectedItem;

                    IsPopupOpen = false;

                    Dispatcher.BeginInvoke(() =>
                    {
                        MoveFocus(
                            new TraversalRequest(
                                FocusNavigationDirection.Next));
                    });
                }

                e.Handled = true;
                break;

            case Key.Escape:

                IsPopupOpen = false;
                PART_SearchBox.Focus();

                e.Handled = true;
                break;
        }
    }



}