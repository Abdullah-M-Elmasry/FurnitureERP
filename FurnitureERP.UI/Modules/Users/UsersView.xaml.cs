using System.Windows.Controls;

namespace FurnitureERP.UI.Modules.Users;

public partial class UsersView : UserControl
{
    public UsersView(UsersViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}