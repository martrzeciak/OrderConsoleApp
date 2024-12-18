using Microsoft.EntityFrameworkCore;
using OrderConsoleApp.Data;

namespace OrderConsoleApp;

public class App(AppDbContext context)
{
    public async Task Run(string[] args)
    {
        var products = await context.Products.ToListAsync();

        foreach (var product in products)
        {
            Console.WriteLine(product.Name);
        }
    }
}