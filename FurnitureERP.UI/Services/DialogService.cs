using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.UI.Common.Dialog;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FurnitureERP.UI.Services;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool ShowDialog<TViewModel>(Action<TViewModel>? configure = null)
        where TViewModel : class
    {
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        configure?.Invoke(vm);

        var viewName = typeof(TViewModel)
            .Name.Replace("ViewModel", "View");

        var viewType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .First(x => x.Name == viewName);

        var view = Activator.CreateInstance(viewType) as FrameworkElement;
        view!.DataContext = vm;

        var window = new Window
        {
            Content = view,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //ResizeMode = ResizeMode.NoResize
        };

        // 🔥 الربط العام لكل الديالوجات
        if (vm is IDialogRequestClose dialogVm)
        {
            dialogVm.RequestClose += result =>
            {
                window.DialogResult = result;
                window.Close();
            };
        }

        return window.ShowDialog() == true;
    }


    public TResult? ShowDialog<TViewModel, TResult>(
    Action<TViewModel>? configure = null)
    where TViewModel : class, IDialogResult<TResult>
    {
        var vm = _serviceProvider.GetRequiredService<TViewModel>();

        configure?.Invoke(vm);

        var viewName = typeof(TViewModel)
            .Name.Replace("ViewModel", "View");

        var viewType = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .First(x => x.Name == viewName);

        var view = Activator.CreateInstance(viewType) as FrameworkElement;

        view!.DataContext = vm;

        var window = new Window
        {
            Content = view,
            SizeToContent = SizeToContent.WidthAndHeight,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //ResizeMode = ResizeMode.NoResize
        };

        if (vm is IDialogRequestClose dialogVm)
        {
            dialogVm.RequestClose += result =>
            {
                window.DialogResult = result;
                window.Close();
            };
        }

        var result = window.ShowDialog();

        if (result != true)
            return default;

        return vm.DialogResult;
    }



    // Confirm Dialog باستخدام MaterialDesign
    public async Task<bool> Confirm(string message, string title)
    {
        var vm = new ConfirmDialogViewModel(message);

        bool result = false;

        vm.CloseAction = (r) =>
        {
            result = r;
            MaterialDesignThemes.Wpf.DialogHost.Close("RootDialog");
        };

        var view = new ConfirmDialogView
        {
            DataContext = vm
        };

        await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");

        return result;
    }
}