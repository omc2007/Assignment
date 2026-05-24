using ReCAI.ViewModels;

namespace ReCAI.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage(SignInViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
