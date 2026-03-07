using Avalonia.Controls;
using Avalonia.Interactivity;
using PCBuilder.ViewModels;

namespace PCBuilder.Views;

public partial class MainWindow : Window
{
    private BuilderView? _builderView;
    private SavedBuildsView? _savedBuildsView;
    private MainWindowViewModel? _vm;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel vm, BuilderView builderView, SavedBuildsView savedBuildsView)
    {
        _vm = vm;
        _builderView = builderView;
        _savedBuildsView = savedBuildsView;
        DataContext = vm;
        InitializeComponent();
        ShowBuilderTab();
    }

    private void ShowBuilderTab()
    {
        if (_builderView == null || _vm == null) return;
        PageContent.Content = _builderView;
        TabBuilder.Classes.Add("Active");
        TabSaved.Classes.Remove("Active");
        _vm.SelectedTabIndex = 0;
    }

    private void ShowSavedTab()
    {
        if (_savedBuildsView == null || _vm == null) return;
        PageContent.Content = _savedBuildsView;
        TabBuilder.Classes.Remove("Active");
        TabSaved.Classes.Add("Active");
        _vm.SelectedTabIndex = 1;
        _ = _savedBuildsView.ViewModel?.LoadAsync();
    }

    private void OnBuilderTabClick(object? sender, RoutedEventArgs e) => ShowBuilderTab();
    private void OnSavedTabClick(object? sender, RoutedEventArgs e) => ShowSavedTab();
}
