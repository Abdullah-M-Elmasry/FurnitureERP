using FurnitureERP.UI.Services.Interfaces;
using FurnitureERP.UI.Shell.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace FurnitureERP.UI.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MainViewModel _mainViewModel;

    public NavigationService(
        IServiceProvider serviceProvider,
        MainViewModel mainViewModel)
    {
        _serviceProvider = serviceProvider;
        _mainViewModel = mainViewModel;
    }

    public async Task NavigateTo<TView>(
     object? parameter = null)
     where TView : class
    {
        var view =
            _serviceProvider.GetRequiredService<TView>();

        _mainViewModel.CurrentView = view;

        if (view is UserControl userControl &&
            userControl.DataContext is INavigationAware vm)
        {
            await vm.OnNavigatedTo(parameter);
        }
    }


}