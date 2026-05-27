using ReCAI.Views;
using ReCAI.ViewModels;
using CommunityToolkit.Maui;
using ReCAI.Services;
using ReCAI.Views;
using ReCAI.ViewModels;
using ZXing.Net.Maui.Controls;

namespace ReCAI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>().UseBarcodeReader().ConfigureFonts(fonts =>
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
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<AdminViewModel>();
        builder.Services.AddSingleton<IUserService, InMemoryUserService>();
        builder.Services.AddTransient<UsersListPage>();
        builder.Services.AddTransient<UsersListViewModel>();
        builder.Services.AddTransient<ReCAI.Views.AccountPage>();
        builder.Services.AddTransient<ReCAI.ViewModels.AccountViewModel>();
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<FirebaseUserService>();
        builder.Services.AddSingleton<IProductService, ProductService>();

        return builder.Build();
    }
}