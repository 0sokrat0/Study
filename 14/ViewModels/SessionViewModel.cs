using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Models;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class SessionViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly int _sessionId;

    [ObservableProperty]
    private Session? _session;

    [ObservableProperty]
    private ObservableCollection<SeatInfo> _seats = new();

    [ObservableProperty]
    private SeatInfo? _selectedSeat;

    [ObservableProperty]
    private bool _hideBooked = false;

    public SessionViewModel(int sessionId)
    {
        _db = App.Database;
        _sessionId = sessionId;
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        Session = await _db.GetSessionByIdAsync(_sessionId);
        await LoadSeatsAsync();
    }

    private async Task LoadSeatsAsync()
    {
        var seats = await _db.GetSessionSeatsAsync(_sessionId);
        var seatInfos = seats.Select(s => new SeatInfo
        {
            Seat = s.Seat,
            IsBooked = s.IsBooked
        }).ToList();

        if (HideBooked)
            seatInfos = seatInfos.Where(s => !s.IsBooked).ToList();

        Seats = new ObservableCollection<SeatInfo>(seatInfos);
    }

    [RelayCommand]
    private void SelectSeat(SeatInfo seat)
    {
        if (seat.IsBooked) return;
        SelectedSeat = seat;
    }

    [RelayCommand]
    private void BookTicket()
    {
        if (SelectedSeat == null) return;
        App.MainWindow.NavigateTo(new BookingViewModel(_sessionId, SelectedSeat.Seat.Id));
    }

    [RelayCommand]
    private void GoBack()
    {
        App.MainWindow.NavigateTo(new MainViewModel());
    }

    partial void OnHideBookedChanged(bool value)
    {
        _ = LoadSeatsAsync();
    }
}

public partial class SeatInfo : ObservableObject
{
    public Seat Seat { get; set; } = null!;
    public bool IsBooked { get; set; }
}
