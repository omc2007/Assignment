using Assignment.ViewModels;

namespace Assignment.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
