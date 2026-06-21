using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReCAI.Services;
using ReCAI.Models;
using Microsoft.Maui.Storage;

namespace ReCAI.ViewModels;

public class SignUpViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly FirebaseUserService firebase;
    private readonly ISessionService session;

    private string firstName;
    private string lastName;
    private string email;
    private string password;
    private string mobile;
    private bool isPassword = true;
    private string errorMessage;

    public string FirstName
    {
        get => firstName;
        set
        {
            firstName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSignUp));
        }
    }

    public string LastName
    {
        get => lastName;
        set
        {
            lastName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSignUp));
        }
    }

    public string Email
    {
        get => email;
        set
        {
            email = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSignUp));
        }
    }

    public string Password
    {
        get => password;
        set
        {
            password = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSignUp));
        }
    }

    public string Mobile
    {
        get => mobile;
        set
        {
            mobile = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanSignUp));
        }
    }

    public bool IsPassword
    {
        get => isPassword;
        set
        {
            isPassword = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => errorMessage;
        set
        {
            errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool CanSignUp =>
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(Mobile);

    public ICommand TogglePasswordCommand { get; }
    public ICommand SignUpCommand { get; }
    public ICommand GoToSignInCommand { get; }

    public SignUpViewModel(IUserService userService, FirebaseUserService firebase, ISessionService session)
    {
        this.userService = userService;
        this.firebase = firebase;
        this.session = session;

        TogglePasswordCommand = new Command(() => IsPassword = !IsPassword);
        SignUpCommand = new Command(SignUp);
        GoToSignInCommand = new Command(GoToSignIn);
    }

    private async void SignUp()
    {
        if (string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(Mobile))
        {
            ErrorMessage = "All fields are required";
            return;
        }

        if (!Email.Contains("@"))
        {
            ErrorMessage = "Invalid email";
            return;
        }

        if (userService.ExistsByEmail(Email))
        {
            ErrorMessage = "User already exists";
            return;
        }

        var newUser = new AppUser
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Mobile = Mobile,
            Password = Password,
            IsAdmin = false,
            CreatedAt = DateTime.Now
        };

        userService.Add(newUser);

        await firebase.AddUser(newUser);

        session.CurrentUserId = newUser.Id;
        session.IsAdmin = false;

        Preferences.Default.Set("userId", newUser.Id);
        Preferences.Default.Set("email", newUser.Email);
        Preferences.Default.Set("userName", newUser.Email);
        Preferences.Default.Set("isAdmin", false);

        ErrorMessage = string.Empty;

        ((AppShell)Shell.Current).SetMenuByUser(newUser.Email);

        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void GoToSignIn()
    {
        await Shell.Current.GoToAsync("//SignInPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}