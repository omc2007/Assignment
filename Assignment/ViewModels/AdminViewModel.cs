using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReCAI.ViewModels;

public class AdminViewModel
{
    public ICommand ViewUsersCommand { get; }

    public AdminViewModel()
    {
        ViewUsersCommand = new Command(ViewUsers);
    }

    private async void ViewUsers()
    {
        await Shell.Current.GoToAsync("//UsersListPage");

    }
}

