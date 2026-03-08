using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RoguelikeGame.Views;

public partial class GameOverView : UserControl
{
    public event EventHandler? Restart;

    public GameOverView(int floorsReached)
    {
        InitializeComponent();
        FloorReachedText.Text = $"Вы достигли этажа: {floorsReached}";
    }

    private void OnRestartClick(object? sender, RoutedEventArgs e)
    {
        Restart?.Invoke(this, EventArgs.Empty);
    }
}
