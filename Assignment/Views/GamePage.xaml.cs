using ReCAI.ViewModels;

namespace ReCAI.Views;

public partial class GamePage : ContentPage
{
    private readonly GameViewModel vm;
    private bool isAnimating;

    public GamePage()
    {
        InitializeComponent();

        vm = new GameViewModel();
        BindingContext = vm;
    }

    private Image? ProductImageControl => this.FindByName<Image>("ProductImage");

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

    private void OnRestartClicked(object sender, EventArgs e)
    {
        vm.RestartGame();
        ResetProductImage();
    }
}