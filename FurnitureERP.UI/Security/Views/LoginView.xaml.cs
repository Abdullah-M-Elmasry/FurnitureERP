using System.Windows;
using FurnitureERP.UI.Security.ViewModels;

namespace FurnitureERP.UI.Security.Views;

public partial class LoginView : Window
{
    private bool _isArabic;

    // ✅ DI هيدخل الـ ViewModel هنا تلقائي
    public LoginView(LoginViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        var vm = (LoginViewModel)DataContext;

        vm.Password = PasswordBox.Password;

        if (vm.LoginCommand.CanExecute(null))
            vm.LoginCommand.Execute(null);
    }

    // UI Logic فقط (مسموح في View)
    private void ToggleLanguage_Click(object sender, RoutedEventArgs e)
    {
        _isArabic = !_isArabic;

        FlowDirection = _isArabic
            ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
    }

    private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }

    private void TextBox_TextChanged_1(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }
}
