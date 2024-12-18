namespace OrderConsoleApp.Entities;

public class OrderItem : BaseEntity
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
