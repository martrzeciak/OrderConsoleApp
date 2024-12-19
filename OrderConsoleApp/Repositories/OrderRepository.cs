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

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await context.Orders.Include(x => x.OrderItems).ToListAsync();
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync()
        {
            return await context.OrderItems
                .ToListAsync();
        }
    }
}
