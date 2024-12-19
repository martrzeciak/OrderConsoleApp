using Microsoft.EntityFrameworkCore;
using OrderConsoleApp.Contracts;
using OrderConsoleApp.Data;
using OrderConsoleApp.Entities;

namespace OrderConsoleApp.Repositories
{
    public class OrderRepository(AppDbContext context) : IOrderRepository
    {
        public void Add(Order order)
        {
            context.Orders.Add(order);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>> GetOrdersAsync()
        {

            var orders = await context.Orders.Include(x => x.OrderItems).ToListAsync();
            return orders;
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync()
        {
            return await context.OrderItems
                .ToListAsync();
        }
    }
}
