using Avalonia.Controls;
using RoguelikeGame.Views;

namespace RoguelikeGame;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        var menu = new MainMenuView();
        menu.StartGame += (_, _) => ShowGame();
        MainContent.Content = menu;
    }

    private void ShowGame()
    {
        var game = new GameView();
        game.GameOver += (_, floor) => ShowGameOver(floor);
        MainContent.Content = game;
    }

    private void ShowGameOver(int floor)
    {
        var over = new GameOverView(floor);
        over.Restart += (_, _) => ShowGame();
        MainContent.Content = over;
    }
}
