using OrderConsoleApp.Contracts;

namespace OrderConsoleApp;

public class App(IProductRepository productRepository)
{
    public async Task Run(string[] args)
    {
        var products = await productRepository.GetProductsAsync();

        foreach (var product in products)
        {
            Console.WriteLine(product.Name);
        }
    }
}