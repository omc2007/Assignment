using Assignment.ViewModels;

namespace Assignment.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class AccountPage : ContentPage
{
    private readonly AccountViewModel vm;

    public string UserId { get; set; } = "";

    public AccountPage(AccountViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        bool isAdmin = true;
        vm.Load(UserId);
    }
}
