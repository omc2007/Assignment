using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ReCAI.Models;
using ReCAI.Services;
using ReCAI.Views;
using Microsoft.Maui.Storage;

namespace ReCAI.ViewModels;

public class AccountViewModel : INotifyPropertyChanged
{
    private const string AdminEmail = "admin@gmail.com";

    private readonly IUserService userService;
    private readonly ISessionService session;
    private readonly GameScoreService gameScoreService = new();

    private string userId = "";
    private string firstName = "";
    private string lastName = "";
    private string email = "";
    private string originalEmail = "";
    private string mobile = "";
    private string errorMessage = "";
    private ImageSource profileImage = "dotnet_bot.png";

    public string UserId
    {
        get => userId;
        set
        {
            userId = value;
            OnPropertyChanged();
        }
    }

    public string FirstName
    {
        get => firstName;
        set
        {
            firstName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanUpdate));
        }
    }

    public string LastName
    {
        get => lastName;
        set
        {
            lastName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanUpdate));
        }
    }

    public string Email
    {
        get => email;
        set
        {
            email = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanUpdate));
            OnPropertyChanged(nameof(IsAdminAccount));
            OnPropertyChanged(nameof(CanEditEmail));
            OnPropertyChanged(nameof(CanDelete));
        }
    }

    public string Mobile
    {
        get => mobile;
        set
        {
            mobile = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanUpdate));
        }
    }

    public ImageSource ProfileImage
    {
        get => profileImage;
        set
        {
            profileImage = value;
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

    public bool IsAdminAccount =>
        originalEmail.Trim().ToLower() == AdminEmail;

    public bool CanEditEmail =>
        !IsAdminAccount;

    public bool CanUpdate =>
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Mobile);

    public bool CanDelete =>
        session.IsAdmin && !IsAdminAccount;

    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }

    public AccountViewModel(IUserService userService, ISessionService session)
    {
        this.userService = userService;
        this.session = session;

        UpdateCommand = new Command(Update);
        DeleteCommand = new Command(Delete);
    }

    public void Load(string id)
    {
        var user = userService.GetById(id);

        if (user == null)
        {
            ErrorMessage = "User not found";
            return;
        }

        UserId = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        originalEmail = user.Email;
        Mobile = user.Mobile;

        ErrorMessage = string.Empty;

        OnPropertyChanged(nameof(IsAdminAccount));
        OnPropertyChanged(nameof(CanEditEmail));
        OnPropertyChanged(nameof(CanDelete));
        OnPropertyChanged(nameof(CanUpdate));
    }

    private async void Update()
    {
        if (IsAdminAccount)
        {
            Email = AdminEmail;
        }

        if (string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Email) ||
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

        var existingUser = userService.GetById(UserId);

        if (existingUser == null)
        {
            ErrorMessage = "User not found";
            return;
        }

        var updatedUser = new AppUser
        {
            Id = existingUser.Id,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Mobile = Mobile,
            Password = existingUser.Password,
            IsAdmin = existingUser.IsAdmin,
            CreatedAt = existingUser.CreatedAt
        };

        userService.Update(updatedUser);

        string displayName = $"{FirstName} {LastName}".Trim();

        if (string.IsNullOrWhiteSpace(displayName))
            displayName = Email;

        await gameScoreService.UpdateUserDetailsAsync(UserId, Email, displayName);

        if (session.CurrentUserId == UserId)
        {
            Preferences.Default.Set("userId", UserId);
            Preferences.Default.Set("email", Email);
            Preferences.Default.Set("userName", displayName);
            Preferences.Default.Set("isAdmin", existingUser.IsAdmin);

            ((AppShell)Shell.Current).SetMenuByUser(Email);
        }

        originalEmail = Email;
        ErrorMessage = string.Empty;

        OnPropertyChanged(nameof(IsAdminAccount));
        OnPropertyChanged(nameof(CanEditEmail));
        OnPropertyChanged(nameof(CanDelete));

        if (session.IsAdmin)
            await Shell.Current.GoToAsync(nameof(UsersListPage));
        else
            await Shell.Current.GoToAsync("//MainPage");
    }

    private async void Delete()
    {
        if (!session.IsAdmin || IsAdminAccount)
            return;

        userService.Delete(UserId);

        await gameScoreService.DeleteScoreByUserIdAsync(UserId);

        await Shell.Current.GoToAsync(nameof(UsersListPage));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}