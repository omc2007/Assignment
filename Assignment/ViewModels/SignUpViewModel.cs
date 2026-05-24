using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReCAI.Services;
using ReCAI.Models;

namespace ReCAI.ViewModels;

public class SignUpViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly FirebaseUserService firebase;
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
        set { firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignUp)); }
    }

    public string LastName
    {
        get => lastName;
        set { lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignUp)); }
    }

    public string Email
    {
        get => email;
        set { email = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignUp)); }
    }

    public string Password
    {
        get => password;
        set { password = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignUp)); }
    }

    public string Mobile
    {
        get => mobile;
        set { mobile = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSignUp)); }
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

    public bool CanSignUp =>
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(Mobile);

    public ICommand TogglePasswordCommand { get; }
    public ICommand SignUpCommand { get; }

    public ICommand GoToSignInCommand { get; }



    public SignUpViewModel(IUserService userService, FirebaseUserService firebase)
    {
        this.userService = userService;
        this.firebase = firebase;

        TogglePasswordCommand = new Command(() => IsPassword = !IsPassword);
        SignUpCommand = new Command(SignUp);
        GoToSignInCommand = new Command(GoToSignIn);
    }

    private async void SignUp()
    {
        // בדיקת אימייל
        if (!Email.Contains("@"))
        {
            ErrorMessage = "Email לא תקין";
            return;
        }

        // בדיקה אם המשתמש כבר קיים (מקומי)
        if (userService.ExistsByEmail(Email))
        {
            ErrorMessage = "המשתמש כבר קיים";
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

        // שמירה מקומית (עדיין עובד אצלך בפרויקט)
        userService.Add(newUser);

        // שמירה ל-Firebase
        await firebase.AddUser(newUser);

        ErrorMessage = "";

        await Shell.Current.GoToAsync("//MainPage");
    }


    public event PropertyChangedEventHandler PropertyChanged;

    // add new user


    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    private async void GoToSignIn()
    {
        await Shell.Current.GoToAsync("//SignInPage");
    }

}

