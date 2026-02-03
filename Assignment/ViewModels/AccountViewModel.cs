using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assignment.Models;
using Assignment.Services;

namespace Assignment.ViewModels;

public class AccountViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly ISessionService session;

    private string userId = "";
    private string firstName = "";
    private string lastName = "";
    private string email = "";
    private string mobile = "";
    private string errorMessage = "";
    private ImageSource profileImage = "dotnet_bot.png";

    public string UserId
    {
        get => userId;
        set { userId = value; OnPropertyChanged(); }
    }

    public string FirstName
    {
        get => firstName;
        set { firstName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanUpdate)); }
    }

    public string LastName
    {
        get => lastName;
        set { lastName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanUpdate)); }
    }

    public string Email
    {
        get => email;
        set { email = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanUpdate)); }
    }

    public string Mobile
    {
        get => mobile;
        set { mobile = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanUpdate)); }
    }

    public ImageSource ProfileImage
    {
        get => profileImage;
        set { profileImage = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => errorMessage;
        set { errorMessage = value; OnPropertyChanged(); }
    }

    public bool CanUpdate =>
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Mobile);

    // ✅ רק Admin יכול למחוק
    public bool CanDelete => session.IsAdmin;

    public ICommand UpdateCommand { get; }
    public ICommand DeleteCommand { get; }

    public AccountViewModel(IUserService userService, ISessionService session)
    {
        this.userService = userService;
        this.session = session;

        UpdateCommand = new Command(Update);
        DeleteCommand = new Command(Delete);
    }

    // ✅ Load רק מקבל userId, הרשאות באות מה-session
    public void Load(string id)
    {
        OnPropertyChanged(nameof(CanDelete));

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
        Mobile = user.Mobile;

        ErrorMessage = "";
    }

    private async void Update()
    {
        if (!Email.Contains("@"))
        {
            ErrorMessage = "Invalid email";
            return;
        }

        userService.Update(new AppUser
        {
            Id = UserId,
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Mobile = Mobile
        });

        ErrorMessage = "";
        if (session.IsAdmin)
            await Shell.Current.GoToAsync("//UsersListPage");
        else
            await Shell.Current.GoToAsync("//MainPage");

    }

    private async void Delete()
    {
        if (!session.IsAdmin)
            return;

        userService.Delete(UserId);
        await Shell.Current.GoToAsync("//UsersListPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


