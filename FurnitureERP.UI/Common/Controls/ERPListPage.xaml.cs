using System.Windows;
using System.Windows.Controls;

namespace FurnitureERP.UI.Common.Controls;

public partial class ERPListPage : UserControl
{
    public ERPListPage()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ERPListPage));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(ERPListPage));

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly DependencyProperty ToolbarContentProperty =
        DependencyProperty.Register(nameof(ToolbarContent), typeof(object), typeof(ERPListPage));

    public object ToolbarContent
    {
        get => GetValue(ToolbarContentProperty);
        set => SetValue(ToolbarContentProperty, value);
    }

    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(object), typeof(ERPListPage));

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly DependencyProperty PaginationContentProperty =
        DependencyProperty.Register(nameof(PaginationContent), typeof(object), typeof(ERPListPage));

    public object PaginationContent
    {
        get => GetValue(PaginationContentProperty);
        set => SetValue(PaginationContentProperty, value);
    }
}