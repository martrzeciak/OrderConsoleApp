using OrderConsoleApp.Entities;

namespace OrderConsoleApp.Contracts;

public interface IProductRepository
{
    Task<List<Product>> GetProductsAsync();
}
