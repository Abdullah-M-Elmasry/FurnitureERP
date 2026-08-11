using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Security.Interfaces;
using FurnitureERP.Domain.Entities.Security;
using FurnitureERP.UI.Core.Commands;
using FurnitureERP.UI.Helpers;
using FurnitureERP.UI.Security.Views;
using FurnitureERP.UI.Shell.Views;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FurnitureERP.UI.Security.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _status = string.Empty;

    private User? _loggedInUser;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public User? LoggedInUser
    {
        get => _loggedInUser;
        private set { _loggedInUser = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService , IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;

        LoginCommand = new RelayCommand(async () => await LoginAsync());
    }

    private async Task LoginAsync()

    {

        //var hasher = _serviceProvider.GetRequiredService<IPasswordHasher>();

        //string hash = hasher.Hash("123456");
        //MessageBox.Show(hash);

        Status = "Checking...";

        //var user = await _authService.LoginAsync(Username, Password);

        //if (user is not null)
        //{
        //    LoggedInUser = user;
        //    Status = $"Welcome {user.FullName} ✅";



        //    //var main = _serviceProvider.GetRequiredService<MainWindow>();
        //    //main.Show();
        //    using var scope = _serviceProvider.CreateScope();
        //    var main = scope.ServiceProvider.GetRequiredService<MainWindow>();
        //    main.Show();

        //    System.Windows.Application.Current.Windows
        //        .OfType<Window>()
        //        .First(w => w is LoginView)
        //        .Close();

        //}
        //else
        //{
        //    Status = "Wrong username or password ❌";
        //}
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
