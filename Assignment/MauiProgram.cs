using Assignment.Views;
using Assignment.ViewModels;
using CommunityToolkit.Maui;
using Assignment.Services;
using Assignment.Views;
using Assignment.ViewModels;
namespace Assignment;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        }).UseMauiCommunityToolkit();
        // ===== Dependency Injection =====
        builder.Services.AddTransient<SignInPage>();
        builder.Services.AddTransient<SignInViewModel>();
        builder.Services.AddTransient<SignUpPage>();
        builder.Services.AddTransient<SignUpViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<AdminViewModel>();
        builder.Services.AddSingleton<IUserService, InMemoryUserService>();
        builder.Services.AddTransient<UsersListPage>();
        builder.Services.AddTransient<UsersListViewModel>();
        builder.Services.AddTransient<Assignment.Views.AccountPage>();
        builder.Services.AddTransient<Assignment.ViewModels.AccountViewModel>();
        builder.Services.AddSingleton<ISessionService, SessionService>();
        return builder.Build();
    }
}