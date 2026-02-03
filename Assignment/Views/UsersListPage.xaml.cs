using Assignment.ViewModels;

namespace Assignment.Views;

public partial class UsersListPage : ContentPage
{
    public UsersListPage(UsersListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
