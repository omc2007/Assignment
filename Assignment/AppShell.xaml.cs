using ReCAI.Views;

namespace ReCAI
{
    public partial class AppShell : Shell
    {
        private const string AdminEmail = "admin@gmail.com";

        public AppShell()
        {
            InitializeComponent();

            // Register pages that are not directly visible in the flyout menu.
            Routing.RegisterRoute(nameof(UsersListPage), typeof(UsersListPage));

            // Default state before login:
            // Admin and Game options are hidden.
            SetMenuByUser(string.Empty);
        }

        public void SetMenuByUser(string email)
        {
            string userEmail = email?.Trim().ToLower() ?? string.Empty;

            bool isLoggedIn = !string.IsNullOrWhiteSpace(userEmail);
            bool isAdmin = userEmail == AdminEmail;

            // Admin sees the Admin page.
            AdminItem.IsVisible = isAdmin;

            // Regular users see the Game page.
            // Admin does not see the Game page.
            GameItem.IsVisible = isLoggedIn && !isAdmin;
            ScanItem.IsVisible = isLoggedIn && !isAdmin;

            // Account stays in the menu, but admin cannot click it.
            AccountItem.IsVisible = isLoggedIn;
            AccountItem.IsEnabled = !isAdmin;
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            // Hide admin and game menu again after logout.
            SetMenuByUser(string.Empty);

            // Navigate back to sign in page.
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }
}