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
            // Admin option is hidden.
            SetMenuByUser(string.Empty);
        }

        public void SetMenuByUser(string email)
        {
            string userEmail = email?.Trim().ToLower() ?? string.Empty;

            bool isAdmin = userEmail == AdminEmail;

            // Only admin sees the Admin page in the menu.
            AdminItem.IsVisible = isAdmin;
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            // Hide admin menu again after logout.
            SetMenuByUser(string.Empty);

            // Navigate back to sign in page.
            await Shell.Current.GoToAsync("//SignInPage");
        }
    }
}