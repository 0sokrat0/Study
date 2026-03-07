using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using PCBuilder.Models;
using PCBuilder.ViewModels;
using PCBuilder.Views;

namespace PCBuilder;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "pcbuilder.db");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            Func<AppDbContext> dbFactory = () => new AppDbContext(options);

            using (var db = dbFactory())
            {
                db.Database.EnsureCreated();
            }

            var builderVm = new BuilderViewModel(dbFactory);
            var savedVm = new SavedBuildsViewModel(dbFactory);
            var mainVm = new MainWindowViewModel(builderVm, savedVm);

            var partSelectFactory = new Func<PartSelectWindow>(() => new PartSelectWindow(dbFactory));
            var builderView = new BuilderView(builderVm, partSelectFactory);
            var savedView = new SavedBuildsView(savedVm);

            desktop.MainWindow = new MainWindow(mainVm, builderView, savedView);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
