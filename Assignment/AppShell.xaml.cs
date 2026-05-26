using ReCAI.Services;
using ReCAI.Views;

namespace ReCAI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(UsersListPage), typeof(UsersListPage));

            UpdateFlyoutItems();
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            UpdateFlyoutItems();
        }

        private ISessionService? GetSessionService()
        {
            return Handler?.MauiContext?.Services.GetService<ISessionService>();
        }

        private void UpdateFlyoutItems()
        {
            var session = GetSessionService();

            AdminItem.IsVisible = session != null && session.IsAdmin;
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;

            var session = GetSessionService();

            session?.SignOut();

            UpdateFlyoutItems();

            await GoToAsync("//SignInPage");
        }
    }
}