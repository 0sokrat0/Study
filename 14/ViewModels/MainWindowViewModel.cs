using CommunityToolkit.Mvvm.ComponentModel;

namespace Cinema.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        _currentPage = new MainViewModel();
    }

    public void NavigateTo(ViewModelBase page)
    {
        CurrentPage = page;
    }
}
