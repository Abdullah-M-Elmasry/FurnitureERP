using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls;

public partial class ERPRowActions : UserControl
{
    public ERPRowActions()
    {
        InitializeComponent();
    }

    public ICommand? ViewCommand
    {
        get => (ICommand?)GetValue(ViewCommandProperty);
        set => SetValue(ViewCommandProperty, value);
    }

    public static readonly DependencyProperty ViewCommandProperty =
        DependencyProperty.Register(
            nameof(ViewCommand),
            typeof(ICommand),
            typeof(ERPRowActions));



    public ICommand? EditCommand
    {
        get => (ICommand?)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public static readonly DependencyProperty EditCommandProperty =
        DependencyProperty.Register(
            nameof(EditCommand),
            typeof(ICommand),
            typeof(ERPRowActions));



    public ICommand? PrintCommand
    {
        get => (ICommand?)GetValue(PrintCommandProperty);
        set => SetValue(PrintCommandProperty, value);
    }

    public static readonly DependencyProperty PrintCommandProperty =
        DependencyProperty.Register(
            nameof(PrintCommand),
            typeof(ICommand),
            typeof(ERPRowActions));



    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public static readonly DependencyProperty DeleteCommandProperty =
        DependencyProperty.Register(
            nameof(DeleteCommand),
            typeof(ICommand),
            typeof(ERPRowActions));



    public bool ShowView
    {
        get => (bool)GetValue(ShowViewProperty);
        set => SetValue(ShowViewProperty, value);
    }

    public static readonly DependencyProperty ShowViewProperty =
        DependencyProperty.Register(
            nameof(ShowView),
            typeof(bool),
            typeof(ERPRowActions),
            new PropertyMetadata(true));



    public bool ShowEdit
    {
        get => (bool)GetValue(ShowEditProperty);
        set => SetValue(ShowEditProperty, value);
    }

    public static readonly DependencyProperty ShowEditProperty =
        DependencyProperty.Register(
            nameof(ShowEdit),
            typeof(bool),
            typeof(ERPRowActions),
            new PropertyMetadata(true));



    public bool ShowPrint
    {
        get => (bool)GetValue(ShowPrintProperty);
        set => SetValue(ShowPrintProperty, value);
    }

    public static readonly DependencyProperty ShowPrintProperty =
        DependencyProperty.Register(
            nameof(ShowPrint),
            typeof(bool),
            typeof(ERPRowActions),
            new PropertyMetadata(true));



    public bool ShowDelete
    {
        get => (bool)GetValue(ShowDeleteProperty);
        set => SetValue(ShowDeleteProperty, value);
    }

    public static readonly DependencyProperty ShowDeleteProperty =
        DependencyProperty.Register(
            nameof(ShowDelete),
            typeof(bool),
            typeof(ERPRowActions),
            new PropertyMetadata(false));
}