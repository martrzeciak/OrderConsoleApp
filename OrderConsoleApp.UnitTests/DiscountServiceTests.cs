using AutoFixture;
using OrderConsoleApp.Entities;
using OrderConsoleApp.Services;

namespace OrderConsoleApp.UnitTests;
public class DiscountServiceTests
{
    private readonly Fixture _fixture;
    private readonly DiscountService _discountService;

    public DiscountServiceTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _discountService = new DiscountService();
    }

    [Fact]
    public void GetDiscount_ShouldReturnZero_WhenNoItemsInOrder()
    {
        // Arrange: Create an order with no items
        var order = _fixture.Build<Order>()
            .With(o => o.OrderItems, []) // Empty OrderItems
            .Create();

        // Act: Get the discount
        var result = _discountService.GetDiscount(order);

        // Assert: Discount should be zero since there are no items
        Assert.Equal(0m, result);
    }

    [Fact]
    public void GetDiscount_ShouldReturnZero_WhenOrderItemsAreNull()
    {
        // Arrange: Create an order with null OrderItems
        var order = _fixture.Build<Order>()
            .With(o => o.OrderItems, (List<OrderItem>)null) // Null OrderItems
            .Create();

        // Act: Get the discount
        var result = _discountService.GetDiscount(order);

        // Assert: Discount should be zero since OrderItems are null
        Assert.Equal(0m, result);
    }

    [Fact]
    public void GetDiscount_ShouldReturnCorrectTotalAfterFivePercentDiscount()
    {
        // Arrange: Create an order where the subtotal exceeds the threshold
        var order = _fixture.Build<Order>()
            .With(o => o.Subtotal, 6000m) // Subtotal greater than 5000
            .Create();

        // Act: Get the discount
        var discount = _discountService.GetDiscount(order);

        // Assert: Ensure that the total is correctly updated after the discount
        Assert.Equal(order.Subtotal - discount, order.Total);
    }

    [Fact]
    public void GetDiscount_ShouldReturnZero_WhenSubtotalBelowThreshold()
    {
        // Arrange: Create an order with a subtotal less than the threshold
        var order = _fixture.Build<Order>()
            .With(o => o.Subtotal, 4999.99m) // Subtotal below 5000
            .Create();

        // Act: Get the discount
        var result = _discountService.GetDiscount(order);

        // Assert: Discount should be zero since subtotal is below the threshold
        Assert.Equal(0m, result);
    }

    [Fact]
    public void GetDiscount_ShouldApplyNoAdditionalDiscount_WhenItemsQuantityGreaterThanOne()
    {
        // Arrange: Create an order with items that all have quantity greater than 1
        var order = _fixture.Build<Order>()
            .With(o => o.OrderItems, new List<OrderItem>
            {
                new OrderItem { Price = 100m, Quantity = 2 },
                new OrderItem { Price = 150m, Quantity = 3 }
            })
            .With(o => o.Subtotal, 250m) // Subtotal is greater than 5000
            .Create();

        // Act: Get the discount
        var discount = _discountService.GetDiscount(order);

        // Assert: No additional discounts should be applied for quantities greater than 1
        Assert.Equal(0m, discount);
    }

    [Fact]
    public void GetDiscount_ShouldCorrectlyCalculateTotal_ForTwoItems_WithQuantityOne()
    {
        // Arrange: Create an order with two items
        var order = _fixture.Build<Order>()
            .With(o => o.OrderItems,
            [
                new OrderItem { Price = 200m, Quantity = 1 },
                new OrderItem { Price = 100m, Quantity = 1 }
            ])
            .Create();

        // Act: Get the discount
        var discount = _discountService.GetDiscount(order);

        // Assert: Ensure the total is correctly calculated after the discount
        Assert.Equal(order.Subtotal - discount, order.Total);
        Assert.Equal(10m, discount);
    }

    [Fact]
    public void GetDiscount_ShouldCorrectlyCalculateTotal_ForThreeItems_WithQuantityOne()
    {
        // Arrange: Create an order which three items
        var order = _fixture.Build<Order>()
            .With(o => o.OrderItems,
            [
                new OrderItem { Price = 200m, Quantity = 1 },
                new OrderItem { Price = 100m, Quantity = 1 },
                new OrderItem { Price = 50m, Quantity = 1 }
            ])
            .Create();

        // Act: Get the discount
        var discount = _discountService.GetDiscount(order);

        // Assert: Ensure the total is correctly calculated after the discount
        Assert.Equal(order.Subtotal - discount, order.Total);
        Assert.Equal(10m, discount);
    }

    [Fact]
    public void GetDiscount_ShouldApplyDiscount_WhenSubtotalIsExactlyThreshold()
    {
        // Arrange: Create an order where the subtotal is exactly 5000
        var order = _fixture.Build<Order>()
            .With(o => o.Subtotal, 5000m) // Exactly at threshold
            .Create();

        // Act: Get the discount
        var discount = _discountService.GetDiscount(order);

        // Assert: No additional discounts should be applied for total of 5000
        Assert.Equal(0m, discount);
    }
}