using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls;

public partial class ERPPagination : UserControl
{
    public ERPPagination()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(
            nameof(CurrentPage),
            typeof(int),
            typeof(ERPPagination));

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }


    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(
            nameof(TotalPages),
            typeof(int),
            typeof(ERPPagination));

    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }


    public static readonly DependencyProperty PreviousCommandProperty =
        DependencyProperty.Register(
            nameof(PreviousCommand),
            typeof(ICommand),
            typeof(ERPPagination));

    public ICommand? PreviousCommand
    {
        get => (ICommand?)GetValue(
            PreviousCommandProperty);

        set => SetValue(
            PreviousCommandProperty,
            value);
    }


    public static readonly DependencyProperty NextCommandProperty =
        DependencyProperty.Register(
            nameof(NextCommand),
            typeof(ICommand),
            typeof(ERPPagination));

    public ICommand? NextCommand
    {
        get => (ICommand?)GetValue(
            NextCommandProperty);

        set => SetValue(
            NextCommandProperty,
            value);
    }


    public static readonly DependencyProperty FirstCommandProperty =
        DependencyProperty.Register(
            nameof(FirstCommand),
            typeof(ICommand),
            typeof(ERPPagination));

    public ICommand? FirstCommand
    {
        get => (ICommand?)GetValue(
            FirstCommandProperty);

        set => SetValue(
            FirstCommandProperty,
            value);
    }


    public static readonly DependencyProperty LastCommandProperty =
        DependencyProperty.Register(
            nameof(LastCommand),
            typeof(ICommand),
            typeof(ERPPagination));

    public ICommand? LastCommand
    {
        get => (ICommand?)GetValue(
            LastCommandProperty);

        set => SetValue(
            LastCommandProperty,
            value);
    }
}