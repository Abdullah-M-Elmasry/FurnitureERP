using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls;

public partial class ERPFormLayout : UserControl
{
    public ERPFormLayout()
    {
        InitializeComponent();
    }

    // =====================================
    // Title
    // =====================================

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ERPFormLayout));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }


    // =====================================
    // Subtitle
    // =====================================

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(
            nameof(Subtitle),
            typeof(string),
            typeof(ERPFormLayout));

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }


    // =====================================
    // Form Content
    // =====================================

    public static readonly DependencyProperty FormContentProperty =
        DependencyProperty.Register(
            nameof(FormContent),
            typeof(object),
            typeof(ERPFormLayout));

    public object FormContent
    {
        get => GetValue(FormContentProperty);
        set => SetValue(FormContentProperty, value);
    }


    // =====================================
    // Save Command
    // =====================================

    public static readonly DependencyProperty SaveCommandProperty =
        DependencyProperty.Register(
            nameof(SaveCommand),
            typeof(ICommand),
            typeof(ERPFormLayout));

    public ICommand SaveCommand
    {
        get => (ICommand)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }


    // =====================================
    // Cancel Command
    // =====================================

    public static readonly DependencyProperty CancelCommandProperty =
        DependencyProperty.Register(
            nameof(CancelCommand),
            typeof(ICommand),
            typeof(ERPFormLayout));

    public ICommand CancelCommand
    {
        get => (ICommand)GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }


    // =====================================
    // Save Button Text
    // =====================================

    public static readonly DependencyProperty SaveButtonTextProperty =
        DependencyProperty.Register(
            nameof(SaveButtonText),
            typeof(string),
            typeof(ERPFormLayout),
            new PropertyMetadata("Save"));

    public string SaveButtonText
    {
        get => (string)GetValue(SaveButtonTextProperty);
        set => SetValue(SaveButtonTextProperty, value);
    }


    // =====================================
    // Loading State
    // =====================================

    public static readonly DependencyProperty IsBusyProperty =
        DependencyProperty.Register(
            nameof(IsBusy),
            typeof(bool),
            typeof(ERPFormLayout),
            new PropertyMetadata(false));

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }


    // =====================================
    // Show Cancel Button
    // =====================================

    public static readonly DependencyProperty ShowCancelProperty =
        DependencyProperty.Register(
            nameof(ShowCancel),
            typeof(bool),
            typeof(ERPFormLayout),
            new PropertyMetadata(true));

    public bool ShowCancel
    {
        get => (bool)GetValue(ShowCancelProperty);
        set => SetValue(ShowCancelProperty, value);
    }
}