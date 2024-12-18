using OrderConsoleApp.Models;

namespace OrderConsoleApp.Contracts;

public interface IProductRepository
{
    Task<List<Product>> GetProductsAsync();
}
