using Microsoft.EntityFrameworkCore;
using OrderConsoleApp.Contracts;
using OrderConsoleApp.Data;
using OrderConsoleApp.Models;

namespace OrderConsoleApp.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<List<Product>> GetProductsAsync()
    {
        return await context.Products.ToListAsync();
    }
}
