using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Models;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly AuthService _auth;

    [ObservableProperty]
    private User? _user;

    [ObservableProperty]
    private ObservableCollection<TicketInfo> _tickets = new();

    public ProfileViewModel()
    {
        _db = App.Database;
        _auth = App.Auth;
        User = _auth.CurrentUser;
        _ = LoadTicketsAsync();
    }

    private async Task LoadTicketsAsync()
    {
        if (User == null) return;

        var tickets = await _db.GetUserTicketsAsync(User.Id);
        Tickets = new ObservableCollection<TicketInfo>(
            tickets.Select(t => new TicketInfo
            {
                Ticket = t.Ticket,
                Movie = t.Movie,
                Session = t.Session,
                Seat = t.Seat,
                Hall = t.Hall
            }));
    }

    [RelayCommand]
    private void Logout()
    {
        _auth.Logout();
        App.MainWindow.NavigateTo(new MainViewModel());
    }

    [RelayCommand]
    private void GoBack()
    {
        App.MainWindow.NavigateTo(new MainViewModel());
    }
}

public class TicketInfo
{
    public Ticket Ticket { get; set; } = null!;
    public Movie Movie { get; set; } = null!;
    public Session Session { get; set; } = null!;
    public Seat Seat { get; set; } = null!;
    public Hall Hall { get; set; } = null!;
}
