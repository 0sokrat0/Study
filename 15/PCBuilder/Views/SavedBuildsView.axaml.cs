using Avalonia.Controls;
using Avalonia.Interactivity;
using PCBuilder.ViewModels;

namespace PCBuilder.Views;

public partial class SavedBuildsView : UserControl
{
    public SavedBuildsViewModel? ViewModel => DataContext as SavedBuildsViewModel;

    public SavedBuildsView()
    {
        InitializeComponent();
    }

    public SavedBuildsView(SavedBuildsViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
    }

    private void OnRefreshClick(object? sender, RoutedEventArgs e)
    {
        _ = ViewModel?.LoadAsync();
    }

    private async void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        var vm = ViewModel;
        if (vm?.SelectedAssembly == null) return;

        var confirmDialog = new Window
        {
            Title = "Подтверждение",
            Width = 360,
            Height = 160,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var panel = new StackPanel { Margin = new Avalonia.Thickness(24) };
        panel.Children.Add(new TextBlock
        {
            Text = $"Удалить сборку «{vm.SelectedAssembly.Name}»?",
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 0, 0, 16)
        });

        var btnRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right, Spacing = 8 };
        var btnCancel = new Button { Content = "Отмена" };
        var btnOk = new Button { Content = "Удалить", Background = Avalonia.Media.Brushes.DarkRed, Foreground = Avalonia.Media.Brushes.White };
        btnCancel.Click += (_, _) => confirmDialog.Close(false);
        btnOk.Click += (_, _) => confirmDialog.Close(true);
        btnRow.Children.Add(btnCancel);
        btnRow.Children.Add(btnOk);
        panel.Children.Add(btnRow);
        confirmDialog.Content = panel;

        var result = await confirmDialog.ShowDialog<bool>(TopLevel.GetTopLevel(this) as Window ?? throw new InvalidOperationException());
        if (result)
            await vm.DeleteAssemblyAsync(vm.SelectedAssembly.Id);
    }
}
