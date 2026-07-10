using System.Windows;
using System.Windows.Controls;

namespace FurnitureERP.UI.Common.Controls;

public partial class ERPPageLayout : UserControl
{
    public ERPPageLayout()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ERPPageLayout),
            new PropertyMetadata(""));



    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(
            nameof(Subtitle),
            typeof(string),
            typeof(ERPPageLayout),
            new PropertyMetadata(""));



    public object ToolbarContent
    {
        get => GetValue(ToolbarContentProperty);
        set => SetValue(ToolbarContentProperty, value);
    }

    public static readonly DependencyProperty ToolbarContentProperty =
        DependencyProperty.Register(
            nameof(ToolbarContent),
            typeof(object),
            typeof(ERPPageLayout),
            new PropertyMetadata(null));



    public object ContentArea
    {
        get => GetValue(ContentAreaProperty);
        set => SetValue(ContentAreaProperty, value);
    }

    public static readonly DependencyProperty ContentAreaProperty =
        DependencyProperty.Register(
            nameof(ContentArea),
            typeof(object),
            typeof(ERPPageLayout),
            new PropertyMetadata(null));
}