using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Models;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class BookingViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly AuthService _auth;
    private readonly int _sessionId;
    private readonly int _seatId;

    [ObservableProperty]
    private Movie? _movie;

    [ObservableProperty]
    private Session? _session;

    [ObservableProperty]
    private Seat? _seat;

    [ObservableProperty]
    private Hall? _hall;

    [ObservableProperty]
    private string _error = string.Empty;

    public BookingViewModel(int sessionId, int seatId)
    {
        _db = App.Database;
        _auth = App.Auth;
        _sessionId = sessionId;
        _seatId = seatId;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        Session = await _db.GetSessionByIdAsync(_sessionId);
        if (Session != null)
        {
            Movie = await _db.GetMovieByIdAsync(Session.MovieId);
            var sessions = await _db.GetMovieSessionsAsync(Session.MovieId);
            Hall = sessions.FirstOrDefault(s => s.Session.Id == _sessionId).Hall;
        }

        var seats = await _db.GetSessionSeatsAsync(_sessionId);
        Seat = seats.FirstOrDefault(s => s.Seat.Id == _seatId).Seat;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        Error = string.Empty;
        if (_auth.CurrentUser == null) return;

        var success = await _db.BookTicketAsync(_auth.CurrentUser.Id, _sessionId, _seatId);
        if (!success)
        {
            Error = "Не удалось забронировать билет";
            return;
        }

        App.MainWindow.NavigateTo(new MainViewModel());
    }

    [RelayCommand]
    private void GoBack()
    {
        App.MainWindow.NavigateTo(new SessionViewModel(_sessionId));
    }
}
