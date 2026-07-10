using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.Application.Customers.Interfaces;
using FurnitureERP.Domain.Entities.Customers;
using FurnitureERP.UI.Common.ViewModels;

namespace FurnitureERP.UI.Modules.Customers.ViewModels;

public partial class CustomersViewModel : CrudListViewModel<Customer>
{
    private readonly ICustomerService _customerService;
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;

    public CustomersViewModel(
        ICustomerService customerService,
        INotificationService notificationService,
        IDialogService dialogService)
    {
        _customerService = customerService;
        _notificationService = notificationService;
        _dialogService = dialogService;
    }

    // =========================
    // LOAD DATA
    // =========================
    public override async Task Load(bool append = false)
    {
        if (IsLoading)
            return;

        try
        {
            IsLoading = true;

            var result = await _customerService.GetAll(
                SearchText ?? "",
                CurrentPage,
                PageSize);

            if (!append)
                Items.Clear();

            foreach (var item in result.Items)
            {
                Items.Add(item);
            }

            TotalPages =
                (int)Math.Ceiling(result.TotalCount / (double)PageSize);

            HasMoreItems = CurrentPage < TotalPages;
        }
        finally
        {
            IsLoading = false;
        }
    }

    // =========================
    // ADD
    // =========================
    [RelayCommand]
    private async Task OpenAddCustomer()
    {
        var saved = _dialogService.ShowDialog<AddCustomerViewModel>();

        if (saved)
            await Refresh();
    }

    // =========================
    // EDIT
    // =========================
    [RelayCommand]
    private async Task EditCustomer(Customer customer)
    {
        if (customer == null)
            return;

        var saved = _dialogService.ShowDialog<AddCustomerViewModel>(
            vm => vm.SetEntity(customer));

        if (saved)
            await Refresh();
    }

    // =========================
    // DELETE
    // =========================
    [RelayCommand]
    private async Task DeleteCustomer(Customer customer)
    {
        if (customer == null)
            return;

        if (!await _dialogService.Confirm(
            $"Delete {customer.Name} ?",
            "Confirm Delete"))
            return;

        await _customerService.Delete(customer.Id);

        Items.Remove(customer);

        _notificationService.ShowSuccess(
            "Customer deleted successfully");
    }
}