using ReCAI.ViewModels;

namespace ReCAI.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
