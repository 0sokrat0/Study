using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class RegisterViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly AuthService _auth;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _error = string.Empty;

    public RegisterViewModel()
    {
        _db = App.Database;
        _auth = App.Auth;
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        Error = string.Empty;
        var user = await _db.RegisterAsync(Email, Password, Name);
        if (user == null)
        {
            Error = "Email уже занят";
            return;
        }

        _auth.Login(user);
        App.MainWindow.NavigateTo(new MainViewModel());
    }

    [RelayCommand]
    private void GoToLogin()
    {
        App.MainWindow.NavigateTo(new LoginViewModel());
    }

    [RelayCommand]
    private void GoBack()
    {
        App.MainWindow.NavigateTo(new MainViewModel());
    }
}
