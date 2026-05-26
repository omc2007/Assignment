using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReCAI.Services;

namespace ReCAI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ISessionService session;

    private string greeting = "Hello";

    public string Greeting
    {
        get => greeting;
        set
        {
            greeting = value;
            OnPropertyChanged();
        }
    }

    public ICommand GoHomeCommand { get; }
    public ICommand GoAdminCommand { get; }
    public ICommand GoAccountCommand { get; }
    public ICommand SignOutCommand { get; }

    public MainViewModel(ISessionService session)
    {
        this.session = session;

        GoHomeCommand = new Command(async () => await GoHomeAsync());
        GoAdminCommand = new Command(async () => await GoAdminAsync());
        GoAccountCommand = new Command(async () => await GoAccountAsync());
        SignOutCommand = new Command(async () => await SignOutAsync());
    }

    private async Task GoHomeAsync()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async Task GoAdminAsync()
    {
        if (!session.IsAdmin)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Access denied",
                "Admin only",
                "OK");

            return;
        }

        await Shell.Current.GoToAsync("//AdminPage");
    }

    private async Task GoAccountAsync()
    {
        if (string.IsNullOrWhiteSpace(session.CurrentUserId))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Session error",
                "No connected user was found. Please sign in again.",
                "OK");

            await Shell.Current.GoToAsync("//SignInPage");
            return;
        }

        string encodedUserId = Uri.EscapeDataString(session.CurrentUserId);
        await Shell.Current.GoToAsync($"//AccountPage?userId={encodedUserId}");
    }

    private async Task SignOutAsync()
    {
        session.SignOut();
        await Shell.Current.GoToAsync("//SignInPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}