using FurnitureERP.UI.Common.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FurnitureERP.UI.Common.Controls.Lookup;

    public partial class ERPAdvancedLookup
    {
        public IEnumerable? ItemsSource
        {
            get => (IEnumerable?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata(null, OnItemsChanged));

        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(ERPAdvancedLookup),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedChanged));

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(
                nameof(SearchText),
                typeof(string),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata(string.Empty));

        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }

        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(
                nameof(Hint),
                typeof(string),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata("Search"));

        public string SearchMemberPaths
        {
            get => (string)GetValue(SearchMemberPathsProperty);
            set => SetValue(SearchMemberPathsProperty, value);
        }

        public static readonly DependencyProperty SearchMemberPathsProperty =
            DependencyProperty.Register(
                nameof(SearchMemberPaths),
                typeof(string),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata("Code,Name"));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(
                nameof(DisplayMemberPath),
                typeof(string),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata("Name"));

        public DataTemplate? ItemTemplate
        {
            get => (DataTemplate?)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(ERPAdvancedLookup));

        public bool IsPopupOpen
        {
            get => (bool)GetValue(IsPopupOpenProperty);
            set => SetValue(IsPopupOpenProperty, value);
        }

        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register(
                nameof(IsPopupOpen),
                typeof(bool),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata(false));

        public ObservableRangeCollection<object> FilteredItems
        {
            get => (ObservableRangeCollection<object>)GetValue(FilteredItemsProperty);
            set => SetValue(FilteredItemsProperty, value);
        }

        public static readonly DependencyProperty FilteredItemsProperty =
            DependencyProperty.Register(
                nameof(FilteredItems),
                typeof(ObservableRangeCollection<object>),
                typeof(ERPAdvancedLookup));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(
                nameof(AddCommand),
                typeof(ICommand),
                typeof(ERPAdvancedLookup));

    public ICommand? EditCommand
    {
        get => (ICommand?)GetValue(EditCommandProperty);
        set => SetValue(EditCommandProperty, value);
    }

    public static readonly DependencyProperty EditCommandProperty =
        DependencyProperty.Register(
            nameof(EditCommand),
            typeof(ICommand),
            typeof(ERPAdvancedLookup));

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public static readonly DependencyProperty DeleteCommandProperty =
        DependencyProperty.Register(
            nameof(DeleteCommand),
            typeof(ICommand),
            typeof(ERPAdvancedLookup));


    public bool ShowAddButton
        {
            get => (bool)GetValue(ShowAddButtonProperty);
            set => SetValue(ShowAddButtonProperty, value);
        }

        public static readonly DependencyProperty ShowAddButtonProperty =
            DependencyProperty.Register(
                nameof(ShowAddButton),
                typeof(bool),
                typeof(ERPAdvancedLookup),
                new PropertyMetadata(false));


    public bool ShowEditButton
    {
        get => (bool)GetValue(ShowEditButtonProperty);
        set => SetValue(ShowEditButtonProperty, value);
    }

    public static readonly DependencyProperty ShowEditButtonProperty =
        DependencyProperty.Register(
            nameof(ShowEditButton),
            typeof(bool),
            typeof(ERPAdvancedLookup),
            new PropertyMetadata(false));

    public bool ShowDeleteButton
    {
        get => (bool)GetValue(ShowDeleteButtonProperty);
        set => SetValue(ShowDeleteButtonProperty, value);
    }

    public static readonly DependencyProperty ShowDeleteButtonProperty =
        DependencyProperty.Register(
            nameof(ShowDeleteButton),
            typeof(bool),
            typeof(ERPAdvancedLookup),
            new PropertyMetadata(false));



    public bool ShowLabel
    {
        get => (bool)GetValue(ShowLabelProperty);
        set => SetValue(ShowLabelProperty, value);
    }

    public static readonly DependencyProperty ShowLabelProperty =
        DependencyProperty.Register(
            nameof(ShowLabel),
            typeof(bool),
            typeof(ERPAdvancedLookup),
            new PropertyMetadata(true));







    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(
            nameof(Label),
            typeof(string),
            typeof(ERPAdvancedLookup),
            new PropertyMetadata(string.Empty));
    
    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    public static readonly DependencyProperty IsRequiredProperty =
        DependencyProperty.Register(
            nameof(IsRequired),
            typeof(bool),
            typeof(ERPAdvancedLookup),
            new PropertyMetadata(false));
    
    public string EmptyText
    {
        get => (string)GetValue(EmptyTextProperty);
        set => SetValue(EmptyTextProperty, value);
    }

    public static readonly DependencyProperty EmptyTextProperty =
        DependencyProperty.Register(
            nameof(EmptyText),
            typeof(string),
            typeof(ERPAdvancedLookup),
            new PropertyMetadata("Create New"));


}



