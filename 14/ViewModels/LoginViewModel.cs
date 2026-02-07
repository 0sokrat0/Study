using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Cinema.Services;

namespace Cinema.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly DatabaseService _db;
    private readonly AuthService _auth;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _error = string.Empty;

    public LoginViewModel()
    {
        _db = App.Database;
        _auth = App.Auth;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        Error = string.Empty;
        var user = await _db.LoginAsync(Email, Password);
        if (user == null)
        {
            Error = "Неверный email или пароль";
            return;
        }

        _auth.Login(user);
        App.MainWindow.NavigateTo(new MainViewModel());
    }

    [RelayCommand]
    private void GoToRegister()
    {
        App.MainWindow.NavigateTo(new RegisterViewModel());
    }

    [RelayCommand]
    private void GoBack()
    {
        App.MainWindow.NavigateTo(new MainViewModel());
    }
}
