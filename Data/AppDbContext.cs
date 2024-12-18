using Microsoft.EntityFrameworkCore;
using OrderConsoleApp.Entities;

namespace OrderConsoleApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial data for the Products table
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 2500 },
            new Product { Id = 2, Name = "Klawiatura", Price = 120 },
            new Product { Id = 3, Name = "Mysz", Price = 90 },
            new Product { Id = 4, Name = "Monitor", Price = 1000 },
            new Product { Id = 5, Name = "Kaczka debuggująca", Price = 66 }
        );
    }
}
