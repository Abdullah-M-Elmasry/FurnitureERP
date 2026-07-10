using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls.Lookup;

public partial class ERPAdvancedLookup
{
    private void OpenPopup(object sender, RoutedEventArgs e)
    {
        ApplyFilter();
        IsPopupOpen = true;
    }

    private void TogglePopup(object sender, RoutedEventArgs e)
    {
        ApplyFilter();

        IsPopupOpen = !IsPopupOpen;

        PART_SearchBox.Focus();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter();
        IsPopupOpen = true;
    }
    private void SearchBox_LostKeyboardFocus(
         object sender,
         KeyboardFocusChangedEventArgs e)
    {
        Dispatcher.BeginInvoke(() =>
        {
            var focused =
                Keyboard.FocusedElement as DependencyObject;

            if (focused == null)
            {
                IsPopupOpen = false;
                return;
            }

            if (!IsAncestorOf(focused))
                IsPopupOpen = false;
        });
    }


}