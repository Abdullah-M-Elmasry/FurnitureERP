using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

public partial class ConfirmDialogViewModel : ObservableObject
{
    public string Message { get; }

    public Action<bool>? CloseAction { get; set; }

    public ConfirmDialogViewModel(string message)
    {
        Message = message;
    }

    [RelayCommand]
    private void Confirm()
    {
        CloseAction?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseAction?.Invoke(false);
    }
}