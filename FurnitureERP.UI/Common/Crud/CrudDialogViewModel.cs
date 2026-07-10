using CommunityToolkit.Mvvm.Input;
using FurnitureERP.Application.Common.Interfaces;
using FurnitureERP.UI.Common.Dialog;

namespace FurnitureERP.UI.Common.Crud;

public abstract partial class CrudDialogViewModel<T>
    : DialogViewModelBase
{
    private readonly INotificationService _notificationService;

    protected CrudDialogViewModel(
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public bool IsEditMode => Entity != null;

    protected T? Entity { get; set; }

    public void SetEntity(T entity)
    {
        Entity = entity;

        LoadEntity(entity);

        OnPropertyChanged(nameof(IsEditMode));
    }

    protected abstract void LoadEntity(T entity);

    protected abstract Task SaveEntity();

    protected virtual string SuccessMessage =>
        IsEditMode
        ? "Updated successfully"
        : "Created successfully";


    [RelayCommand]
    public async Task Save()
    {
        //if (!Validate())
        //    return;

        if (!await ValidateAsync())
            return;

        await SaveEntity();

        _notificationService.ShowSuccess(
            SuccessMessage);

        Close(true);
    }
    //protected new virtual bool Validate()
    //{
    //    return true;
    //}
    protected virtual Task<bool> ValidateAsync()
    {
        ValidateAllProperties();

        return Task.FromResult(!HasErrors);
    }


    [RelayCommand]
    public void Cancel()
    {
        Close(false);
    }
}