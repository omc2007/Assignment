using ReCAI.ViewModels;
using ReCAI.Models;
using ReCAI.Services;
using Microsoft.Maui.Storage;



namespace ReCAI.Views;

public partial class GamePage : ContentPage
{
    private readonly GameViewModel vm;
    private readonly GameScoreService gameScoreService = new();

    private bool isAnimating;
    private int correctAnswersCount;

    private const int TotalQuestions = 10;

    public GamePage()
    {
        InitializeComponent();

        vm = new GameViewModel();
        BindingContext = vm;
    }

    private Image? ProductImageControl => this.FindByName<Image>("ProductImage");

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadTopScoresAsync();
    }

    private async void OnBinClicked(object sender, EventArgs e)
    {
        if (isAnimating)
            return;

        if (sender is not ImageButton selectedButton)
            return;

        string selectedBin = selectedButton.CommandParameter?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(selectedBin))
            return;

        isAnimating = true;

        bool isCorrect = vm.CheckAnswer(selectedBin);

        if (isCorrect)
        {
            await AnimateCorrectAnswer(selectedButton);

            await Task.Delay(350);

            correctAnswersCount++;

            if (correctAnswersCount >= TotalQuestions)
            {
                await FinishGameAsync();

                isAnimating = false;
                return;
            }

            vm.MoveToNextItem();

            ResetProductImage();
        }
        else
        {
            await AnimateWrongAnswer(selectedButton);
        }

        isAnimating = false;
    }

    private async Task AnimateCorrectAnswer(ImageButton selectedButton)
    {
        Image? productImage = ProductImageControl;

        if (productImage == null)
            return;

        await selectedButton.ScaleTo(1.12, 120);
        await selectedButton.ScaleTo(1.0, 120);

        await productImage.ScaleTo(0.75, 150);
        await productImage.TranslateTo(0, 140, 300);
        await productImage.ScaleTo(0.1, 250);
        await productImage.FadeTo(0, 150);
    }

    private async Task AnimateWrongAnswer(ImageButton selectedButton)
    {
        Image? productImage = ProductImageControl;

        if (productImage == null)
            return;

        await selectedButton.RotateTo(-8, 70);
        await selectedButton.RotateTo(8, 70);
        await selectedButton.RotateTo(0, 70);

        await productImage.TranslateTo(-14, 0, 50);
        await productImage.TranslateTo(14, 0, 50);
        await productImage.TranslateTo(-10, 0, 50);
        await productImage.TranslateTo(10, 0, 50);
        await productImage.TranslateTo(0, 0, 50);
    }

    private async Task FinishGameAsync()
    {
        string userId = Preferences.Default.Get("userId", string.Empty);

        if (string.IsNullOrWhiteSpace(userId))
            userId = Preferences.Default.Get("UserId", string.Empty);

        string userEmail = Preferences.Default.Get("email", string.Empty);

        if (string.IsNullOrWhiteSpace(userEmail))
            userEmail = Preferences.Default.Get("UserEmail", string.Empty);

        string userName = Preferences.Default.Get("userName", string.Empty);

        if (string.IsNullOrWhiteSpace(userName))
            userName = Preferences.Default.Get("UserName", string.Empty);

        if (string.IsNullOrWhiteSpace(userName))
            userName = userEmail;

        if (string.IsNullOrWhiteSpace(userId))
            userId = userEmail;

        var score = new GameScore
        {
            UserId = userId,
            UserEmail = userEmail,
            UserName = userName,
            LastScore = vm.Score,
            TotalQuestions = TotalQuestions,
            UpdatedAt = DateTime.Now
        };

        await gameScoreService.SaveScoreAsync(score);

        await DisplayAlert("Game Finished", $"Your score is: {vm.Score}", "OK");

        await LoadTopScoresAsync();

        RestartGame();
    }

    private async Task LoadTopScoresAsync()
    {
        var topScores = await gameScoreService.GetTop3RanksAsync();

        CollectionView? topScoresView = this.FindByName<CollectionView>("TopScoresCollectionView");

        if (topScoresView == null)
            return;

        topScoresView.ItemsSource = topScores;
    }

    private void ResetProductImage()
    {
        Image? productImage = ProductImageControl;

        if (productImage == null)
            return;

        productImage.TranslationX = 0;
        productImage.TranslationY = 0;
        productImage.Scale = 1;
        productImage.Opacity = 1;
        productImage.Rotation = 0;
    }

    private void RestartGame()
    {
        correctAnswersCount = 0;

        vm.RestartGame();

        ResetProductImage();
    }

    private void OnRestartClicked(object sender, EventArgs e)
    {
        RestartGame();
    }
}