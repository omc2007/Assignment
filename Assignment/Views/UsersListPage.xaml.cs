using ReCAI.ViewModels;

namespace ReCAI.Views;

public partial class UsersListPage : ContentPage
{
    public UsersListPage(UsersListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
