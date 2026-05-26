using ReCAI.Services;
using ReCAI.ViewModels;

namespace ReCAI.Views;

public partial class AccountPage : ContentPage, IQueryAttributable
{
    private readonly AccountViewModel vm;
    private readonly ISessionService session;

    private bool loadedFromQuery = false;

    public AccountPage(AccountViewModel vm, ISessionService session)
    {
        InitializeComponent();

        this.vm = vm;
        this.session = session;

        BindingContext = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("userId", out var val))
        {
            var id = val?.ToString() ?? "";

            if (!string.IsNullOrWhiteSpace(id))
            {
                loadedFromQuery = true;
                vm.Load(id);
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (loadedFromQuery)
        {
            loadedFromQuery = false;
            return;
        }

        if (!string.IsNullOrWhiteSpace(session.CurrentUserId))
        {
            vm.Load(session.CurrentUserId);
        }
    }
}