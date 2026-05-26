using System.Windows.Input;
using ReCAI.Views;

namespace ReCAI.ViewModels;

public class AdminViewModel
{
    public ICommand ViewUsersCommand { get; }

    public AdminViewModel()
    {
        ViewUsersCommand = new Command(async () => await ViewUsersAsync());
    }

    private async Task ViewUsersAsync()
    {
        await Shell.Current.GoToAsync(nameof(UsersListPage));
    }
}