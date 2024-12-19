namespace OrderConsoleApp.Entities;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
