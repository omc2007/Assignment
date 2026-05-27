using ReCAI.Models;

namespace ReCAI.Services;

public interface IProductService
{
    Product? GetByBarcode(string barcode);
}