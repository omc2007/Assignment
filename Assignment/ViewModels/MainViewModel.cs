using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assignment.Services;

namespace Assignment.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ISessionService session;

    private string greeting = "Hello";

    public string Greeting
    {
        get => greeting;
        set { greeting = value; OnPropertyChanged(); }
    }

    public ICommand GoHomeCommand { get; }
    public ICommand GoAdminCommand { get; }
    public ICommand GoAccountCommand { get; }
    public ICommand SignOutCommand { get; }

    public MainViewModel(ISessionService session)
    {
        this.session = session;

        GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
        GoAdminCommand = new Command(GoAdmin);
        GoAccountCommand = new Command(GoAccount);
        SignOutCommand = new Command(SignOut);
    }

    private async void GoAdmin()
    {
        if (!session.IsAdmin)
        {
            await Application.Current.MainPage.DisplayAlert("Access denied", "Admin only", "OK");
            return;
        }

        await Shell.Current.GoToAsync("//AdminPage");
    }

    private async void GoAccount()
    {
        if (string.IsNullOrWhiteSpace(session.CurrentUserId))
        {
            await Shell.Current.GoToAsync("//SignInPage");
            return;
        }

        await Shell.Current.GoToAsync($"//AccountPage?userId={Uri.EscapeDataString(session.CurrentUserId)}");
    }


  private async void SignOut()
    {
        session.SignOut();   // מנקה משתמש מחובר
        await Shell.Current.GoToAsync("//SignInPage");
    }
    

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
