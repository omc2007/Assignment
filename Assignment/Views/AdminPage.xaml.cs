using Assignment.ViewModels;

namespace Assignment.Views;

public partial class AdminPage : ContentPage
{
    public AdminPage(AdminViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
