namespace OrderConsoleApp.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}
