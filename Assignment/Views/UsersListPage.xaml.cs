using ReCAI.ViewModels;

namespace ReCAI.Views;

public partial class UsersListPage : ContentPage
{
    private readonly UsersListViewModel vm;

    public UsersListPage(UsersListViewModel vm)
    {
        InitializeComponent();

        this.vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        vm.Reload();
    }
}