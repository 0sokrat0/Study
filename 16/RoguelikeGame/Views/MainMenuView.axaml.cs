using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RoguelikeGame.Views;

public partial class MainMenuView : UserControl
{
    public event EventHandler? StartGame;

    public MainMenuView()
    {
        InitializeComponent();
    }

    private void OnStartGameClick(object? sender, RoutedEventArgs e)
    {
        StartGame?.Invoke(this, EventArgs.Empty);
    }
}
