using OrderConsoleApp.Entities;

namespace OrderConsoleApp.Contracts;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersAsync();
    Task<List<OrderItem>> GetOrderItemsAsync();
    void Add(Order order);
}
