using OrderConsoleApp.Contracts;
using OrderConsoleApp.Entities;

namespace OrderConsoleApp.Services;

public class DiscountService : IDiscountService
{
    private const decimal _orderThreshold = 5000m;
    private const decimal _fivePercentDiscount = 0.05m; // 5% discount
    private const decimal _tenPercentDiscount = 0.10m; // 10% discount
    private const decimal _twentyPercentDiscount = 0.20m; // 20% discount

    public decimal GetDiscount(Order order)
    {
        if (order.OrderItems == null || order.OrderItems.Count == 0)
            return 0.0m;

        var discount = 0m;

        if (order.OrderItems.All(x => x.Quantity == 1))
        {
            var sortedItems = order.OrderItems
               .OrderByDescending(item => item.Price)
               .ToList();

            if (sortedItems.Count == 2)
                discount += sortedItems[1].Price * _tenPercentDiscount;

            if (sortedItems.Count == 3)
                discount += sortedItems[2].Price * _twentyPercentDiscount;
        }

        if (order.Subtotal > _orderThreshold)
            discount += order.Subtotal * _fivePercentDiscount;

        order.Total = order.Subtotal - discount;

        return discount;
    }
}
