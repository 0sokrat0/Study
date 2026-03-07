using Avalonia.Controls;
using Avalonia.Interactivity;
using PCBuilder.ViewModels;

namespace PCBuilder.Views;

public partial class BuilderView : UserControl
{
    private Func<PartSelectWindow>? _partSelectFactory;

    public BuilderViewModel? ViewModel => DataContext as BuilderViewModel;

    public BuilderView()
    {
        InitializeComponent();
    }

    public BuilderView(BuilderViewModel vm, Func<PartSelectWindow> partSelectFactory)
    {
        _partSelectFactory = partSelectFactory;
        DataContext = vm;
        InitializeComponent();
    }

    private async void OnCategoryClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        var category = btn.Tag as string;
        if (category == null || _partSelectFactory == null) return;

        var win = _partSelectFactory();
        win.SetCategory(category);
        var owner = TopLevel.GetTopLevel(this) as Window;
        var result = owner != null
            ? await win.ShowDialog<object?>(owner)
            : null;

        if (result is Models.BasePart part)
            ViewModel?.SetComponent(category, part);
    }

    private void OnRemoveComponent(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string category)
            ViewModel?.SetComponent(category, null);
    }

    private void OnClearClick(object? sender, RoutedEventArgs e)
    {
        ViewModel?.ClearAll();
    }

    private async void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        var vm = ViewModel;
        if (vm == null) return;

        if (!vm.Components.Any(c => c.HasPart))
        {
            await ShowMessage("Пустая сборка", "Добавьте хотя бы один компонент перед сохранением.");
            return;
        }

        var dialog = new SaveAssemblyDialog();
        var owner = TopLevel.GetTopLevel(this) as Window;
        var result = owner != null
            ? await dialog.ShowDialog<(string name, string author)?>(owner)
            : null;

        if (result.HasValue)
        {
            bool saved = await vm.SaveAssembly(result.Value.name, result.Value.author);
            if (saved)
                await ShowMessage("Сохранено", "Сборка успешно сохранена!");
        }
    }

    private async Task ShowMessage(string title, string message)
    {
        var dlg = new Window
        {
            Title = title,
            Width = 360,
            Height = 160,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };
        var panel = new StackPanel { Margin = new Avalonia.Thickness(24) };
        panel.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(0, 0, 0, 16)
        });
        var btn = new Button { Content = "ОК", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
        btn.Click += (_, _) => dlg.Close();
        panel.Children.Add(btn);
        dlg.Content = panel;
        var owner = TopLevel.GetTopLevel(this) as Window;
        if (owner != null)
            await dlg.ShowDialog(owner);
    }
}
