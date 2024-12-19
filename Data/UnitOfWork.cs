using OrderConsoleApp.Contracts;
using OrderConsoleApp.Repositories;

namespace OrderConsoleApp.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IOrderRepository OrderRepository => new OrderRepository(context);

    public IProductRepository ProductRepository => new ProductRepository(context);

    public async Task<bool> Complete()
    {
        // Save changes and reset the change tracker
        var changesSaved = await context.SaveChangesAsync() > 0;

        if (changesSaved)
        {
            context.ChangeTracker.Clear(); // Clear the EF change tracker to force a fresh state
        }

        return changesSaved;
    }
}
