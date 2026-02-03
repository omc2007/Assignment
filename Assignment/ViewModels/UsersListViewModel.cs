using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Assignment.Models;
using Assignment.Services;


namespace Assignment.ViewModels;

public class UsersListViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly ISessionService session;
    private string searchText = "";
    private AppUser? selectedUser;

    public ObservableCollection<AppUser> Users { get; } = new();

    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged();
            Reload();
        }
    }

    public AppUser? SelectedUser
    {
        get => selectedUser;
        set
        {
            selectedUser = value;
            OnPropertyChanged();
            if (selectedUser != null)
            {
                OpenUser(selectedUser);
            }
        }
    }

    public ICommand ClearSearchCommand { get; }

    public UsersListViewModel(IUserService userService, ISessionService session)
    {
        this.userService = userService;
        this.session = session;

        ClearSearchCommand = new Command(() => SearchText = "");
        Reload();
    }


    private void Reload()
    {
        Users.Clear();

        var all = userService.GetAll();

        IEnumerable<Models.AppUser> baseList = all;

        // אם לא אדמין, רואה רק את עצמו
        if (!session.IsAdmin && !string.IsNullOrWhiteSpace(session.CurrentUserId))
        {
            baseList = all.Where(u => u.Id == session.CurrentUserId);
        }

        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? baseList
            : baseList.Where(u =>
                u.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var u in filtered)
            Users.Add(u);
    }



    private async void OpenUser(AppUser user)
    {
        SelectedUser = null;

        await Shell.Current.GoToAsync(
            $"//AccountPage?userId={Uri.EscapeDataString(user.Id)}"
        );
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

