using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Models;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly AuthService _auth;

    [ObservableProperty]
    private ObservableCollection<Movie> _movies = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _sortBy = "none";

    public MainViewModel()
    {
        _db = App.Database;
        _auth = App.Auth;
        _ = LoadMoviesAsync();
    }

    [RelayCommand]
    private async Task LoadMoviesAsync()
    {
        var movies = await _db.GetMoviesAsync(
            string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
            SortBy == "none" ? null : SortBy);
        Movies = new ObservableCollection<Movie>(movies);
    }

    [RelayCommand]
    private void SelectMovie(Movie movie)
    {
        App.MainWindow.NavigateTo(new MovieViewModel(movie.Id));
    }

    [RelayCommand]
    private void OpenProfile()
    {
        if (_auth.IsAuthenticated)
            App.MainWindow.NavigateTo(new ProfileViewModel());
        else
            App.MainWindow.NavigateTo(new LoginViewModel());
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = LoadMoviesAsync();
    }

    partial void OnSortByChanged(string value)
    {
        _ = LoadMoviesAsync();
    }
}
