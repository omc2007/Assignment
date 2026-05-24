using ReCAI.ViewModels;



namespace ReCAI.Views;

public partial class AccountPage : ContentPage, IQueryAttributable
{
    private readonly AccountViewModel vm;

    public AccountPage(AccountViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("userId", out var val))
        {
            var id = val?.ToString() ?? "";
            vm.Load(id);
        }
    }
}

