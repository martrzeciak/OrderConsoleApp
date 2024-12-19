namespace OrderConsoleApp.Contracts;

public interface IUnitOfWork
{
    IOrderRepository OrderRepository { get; }
    IProductRepository ProductRepository { get; }
    Task<bool> Complete();
}
