using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Cinema.ViewModels;
using Cinema.Views;
using Cinema.Services;

namespace Cinema;

public partial class App : Application
{
    public static DatabaseService Database { get; private set; } = null!;
    public static AuthService Auth { get; private set; } = null!;
    public static MainWindowViewModel MainWindow { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            try
            {
                DisableAvaloniaDataAnnotationValidation();

                Database = new DatabaseService();
                Auth = new AuthService();
                MainWindow = new MainWindowViewModel();

                await Database.InitializeAsync();
                await Database.SeedDataAsync();

                var window = new MainWindow
                {
                    DataContext = MainWindow
                };
                window.Show();
                desktop.MainWindow = window;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
