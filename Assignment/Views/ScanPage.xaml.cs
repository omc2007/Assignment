using ReCAI.Services;
using ZXing.Net.Maui;

namespace ReCAI.Views;

public partial class ScanPage : ContentPage
{
    private readonly IProductService productService;
    private bool hasScanned = false;

    public ScanPage(IProductService productService)
    {
        InitializeComponent();

        this.productService = productService;

        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    private async void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (hasScanned)
            return;

        string? barcode = e.Results?.FirstOrDefault()?.Value;

        if (string.IsNullOrWhiteSpace(barcode))
            return;

        hasScanned = true;

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var product = productService.GetByBarcode(barcode);

            if (product == null)
            {
                ResultLabel.Text =
                    $"Barcode detected: {barcode}\n" +
                    "Product not found in the local database.";

                BinColorLabel.Text = "";
                BinColorLabel.TextColor = Colors.Black;

                await DisplayAlert(
                    "Product not found",
                    $"Barcode: {barcode}\nThis product is not in the local database.",
                    "OK"
                );

                return;
            }

            ResultLabel.Text =
                $"Product: {product.Name}\n" +
                $"Packaging: {product.Packaging}\n" +
                $"Recommended bin: {product.RecommendedBin}";

            BinColorLabel.Text = $"Bin color: {product.BinColor}";
            BinColorLabel.TextColor = GetColorByName(product.BinColor);

            await DisplayAlert(
                "Product found",
                $"Product: {product.Name}\n" +
                $"Packaging: {product.Packaging}\n" +
                $"Recommended bin: {product.RecommendedBin}\n" +
                $"Bin color: {product.BinColor}",
                "OK"
            );
        });
    }

    private Color GetColorByName(string colorName)
    {
        string cleanColor = colorName.Trim().ToLower();

        return cleanColor switch
        {
            "orange" => Colors.Orange,
            "purple" => Colors.Purple,
            "blue" => Colors.Blue,
            "green" => Colors.Green,
            "brown" => Colors.Brown,
            "gray" => Colors.Gray,
            "grey" => Colors.Gray,
            "black" => Colors.Black,
            _ => Colors.Black
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        hasScanned = false;
        barcodeReader.IsDetecting = true;

        ResultLabel.Text = "Point the camera at a product barcode";
        BinColorLabel.Text = "";
        BinColorLabel.TextColor = Colors.Black;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        barcodeReader.IsDetecting = false;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}