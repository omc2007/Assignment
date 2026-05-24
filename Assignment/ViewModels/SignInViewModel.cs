using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReCAI.Services;
using ReCAI.Models;
namespace ReCAI.ViewModels;



public class SignInViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly ISessionService session;

    private string email;
    private string password;
    private bool isPassword = true;
    private string errorMessage;

    public string Email
    {
        get => email;
        set { email = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignIn)); }
    }

    public string Password
    {
        get => password;
        set { password = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignIn)); }
    }

    public bool IsPassword
    {
        get => isPassword;
        set { isPassword = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => errorMessage;
        set { errorMessage = value; OnPropertyChanged(); }
    }

    public bool CanSignIn =>
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password);

    public ICommand TogglePasswordCommand { get; }
    public ICommand SignInCommand { get; }
    public ICommand GoToSignUpCommand { get; }

    public SignInViewModel(IUserService userService, ISessionService session)
    {
        this.userService = userService;
        this.session = session;

        TogglePasswordCommand = new Command(TogglePassword);
        SignInCommand = new Command(SignIn);
        GoToSignUpCommand = new Command(GoToSignUp);
    }

    private void TogglePassword() => IsPassword = !IsPassword;

    private async void SignIn()
    {
        var user = userService.GetByEmail(Email);
        if (user == null || user.Password != Password)
        {
            ErrorMessage = "Invalid email or password";
            return;
        }

        session.CurrentUserId = user.Id;
        session.IsAdmin = user.IsAdmin;

        ErrorMessage = "";
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void GoToSignUp()
    {
        await Shell.Current.GoToAsync("//SignUpPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}



