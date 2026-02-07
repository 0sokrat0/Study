using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Models;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class MovieViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly int _movieId;

    [ObservableProperty]
    private Movie? _movie;

    [ObservableProperty]
    private string _genres = string.Empty;

    [ObservableProperty]
    private ObservableCollection<SessionInfo> _sessions = new();

    public MovieViewModel(int movieId)
    {
        _db = App.Database;
        _movieId = movieId;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        Movie = await _db.GetMovieByIdAsync(_movieId);
        var genres = await _db.GetMovieGenresAsync(_movieId);
        Genres = string.Join(", ", genres.Select(g => g.Name));

        var sessions = await _db.GetMovieSessionsAsync(_movieId);
        Sessions = new ObservableCollection<SessionInfo>(
            sessions.Select(s => new SessionInfo
            {
                Session = s.Session,
                Hall = s.Hall
            }));
    }

    [RelayCommand]
    private void SelectSession(SessionInfo info)
    {
        if (!App.Auth.IsAuthenticated)
        {
            App.MainWindow.NavigateTo(new LoginViewModel());
            return;
        }
        App.MainWindow.NavigateTo(new SessionViewModel(info.Session.Id));
    }

    [RelayCommand]
    private void GoBack()
    {
        App.MainWindow.NavigateTo(new MainViewModel());
    }
}

public class SessionInfo
{
    public Session Session { get; set; } = null!;
    public Hall Hall { get; set; } = null!;
}
