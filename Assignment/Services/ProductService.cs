using ReCAI.Models;

namespace ReCAI.Services;

public class ProductService : IProductService
{
    private readonly List<Product> products = new()
    {
        new Product
        {
            Barcode = "7290119770199",
            Name = "Ein Gedi Small Water Bottle",
            Packaging = "Plastic bottle",
            RecommendedBin = "Plastic recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "7290004131074",
            Name = "Tnuva 3% Milk Carton",
            Packaging = "Milk carton",
            RecommendedBin = "Carton recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "8410066140190",
            Name = "Heinz Ketchup Bottle",
            Packaging = "Plastic bottle",
            RecommendedBin = "Plastic recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "7290019056942",
            Name = "Ein Gedi Large Water Bottle",
            Packaging = "Plastic bottle",
            RecommendedBin = "Plastic recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "7290017888972",
            Name = "Neviot 1 Liter Water Bottle",
            Packaging = "Plastic bottle",
            RecommendedBin = "Plastic recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "2266002871361",
            Name = "Carlsberg Beer Bottle",
            Packaging = "Glass bottle",
            RecommendedBin = "Glass recycling bin",
            BinColor = "Purple"
        },
        new Product
        {
            Barcode = "7290000144474",
            Name = "Canola Oil Bottle",
            Packaging = "Plastic bottle",
            RecommendedBin = "Plastic recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "8711000601587",
            Name = "Jacobs Coffee Jar",
            Packaging = "Glass jar",
            RecommendedBin = "Glass recycling bin",
            BinColor = "Purple"
        },
        new Product
        {
            Barcode = "7290003643646",
            Name = "Demerara Brown Sugar Container",
            Packaging = "Plastic container",
            RecommendedBin = "Plastic recycling bin",
            BinColor = "Orange"
        },
        new Product
        {
            Barcode = "7290018091111",
            Name = "Maadanot Pizza Box",
            Packaging = "Cardboard box",
            RecommendedBin = "Paper and cardboard recycling bin",
            BinColor = "Blue"
        }
    };

    public Product? GetByBarcode(string barcode)
    {
        string cleanBarcode = barcode.Trim();

        return products.FirstOrDefault(product =>
            product.Barcode == cleanBarcode);
    }
}