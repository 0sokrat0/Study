using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PCBuilder.Views;

public partial class SaveAssemblyDialog : Window
{
    public SaveAssemblyDialog()
    {
        InitializeComponent();
    }

    private void OnSave(object? sender, RoutedEventArgs e)
    {
        var name = NameBox.Text?.Trim() ?? "";
        var author = AuthorBox.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(name))
        {
            NameBox.Focus();
            return;
        }
        Close((name, author));
    }

    private void OnCancel(object? sender, RoutedEventArgs e) => Close(null);
}
