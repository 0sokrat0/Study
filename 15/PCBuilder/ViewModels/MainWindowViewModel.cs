using Avalonia.Controls;
using ReactiveUI;

namespace PCBuilder.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
    }

    private object? _currentView;
    public object? CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    public BuilderViewModel Builder { get; }
    public SavedBuildsViewModel SavedBuilds { get; }

    public MainWindowViewModel(BuilderViewModel builder, SavedBuildsViewModel savedBuilds)
    {
        Builder = builder;
        SavedBuilds = savedBuilds;
    }

    public void SelectTab(object? param)
    {
        if (param is int idx)
            SelectedTabIndex = idx;
        else if (param is string s && int.TryParse(s, out int i))
            SelectedTabIndex = i;
    }
}
