using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace FurnitureERP.UI.Common.ViewModels;

public abstract partial class CrudListViewModel<T>
    : ObservableObject
{
    // =========================
    // DATA
    // =========================

    [ObservableProperty]
    private ObservableCollection<T> items = new();

    [ObservableProperty]
    private T? selectedItem;


    // =========================
    // UI STATE
    // =========================

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? searchText;


    // =========================
    // PAGINATION
    // =========================

    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int pageSize = 20;

    [ObservableProperty]
    private bool hasMoreItems = true;

    [ObservableProperty]
    private int totalPages;

    protected CancellationTokenSource? _searchCts;


    // =========================
    // SEARCH
    // =========================

    partial void OnSearchTextChanged(string? value)
    {
        CurrentPage = 1;
        HasMoreItems = true;

        _searchCts?.Cancel();

        _searchCts = new CancellationTokenSource();

        _ = DebouncedSearch(
            _searchCts.Token);
    }

    private async Task DebouncedSearch(
        CancellationToken token)
    {
        try
        {
            await Task.Delay(
                100,
                token);

            await Load();
        }
        catch
        {

        }
    }


    // =========================
    // ABSTRACT
    // =========================

    public abstract Task Load(
        bool append = false);


    // =========================
    // GENERAL ACTIONS
    // =========================

    public virtual void Reset()
    {
        CurrentPage = 1;

        HasMoreItems = true;

        Items.Clear();
    }


    [RelayCommand]
    public virtual async Task Refresh()
    {
        CurrentPage = 1;

        HasMoreItems = true;

        await Load();
    }


    // =========================
    // PAGINATION COMMANDS
    // =========================
    [RelayCommand]
    public virtual async Task NextPage()
    {
        if (CurrentPage >= TotalPages)
            return;

        CurrentPage++;

        await Load();
    }


    [RelayCommand]
    public virtual async Task PreviousPage()
    {
        if (CurrentPage <= 1)
            return;

        CurrentPage--;

        await Load();
    }


    [RelayCommand]
    public virtual async Task FirstPage()
    {
        CurrentPage = 1;

        await Load();
    }


    [RelayCommand]
    public virtual async Task LastPage()
    {
        CurrentPage = TotalPages;

        await Load();
    }
}