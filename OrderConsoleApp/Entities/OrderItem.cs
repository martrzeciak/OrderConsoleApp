namespace OrderConsoleApp.Entities;

public class OrderItem : BaseEntity
{
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;
}
