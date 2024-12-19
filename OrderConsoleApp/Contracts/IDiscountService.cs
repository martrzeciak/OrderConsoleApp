using OrderConsoleApp.Entities;

namespace OrderConsoleApp.Contracts;

public interface IDiscountService
{
    decimal GetDiscount(Order order);
}
