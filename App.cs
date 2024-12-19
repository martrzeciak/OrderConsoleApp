using OrderConsoleApp.Contracts;
using OrderConsoleApp.Entities;

namespace OrderConsoleApp;

public class App(IUnitOfWork unitOfWork)
{
    private const string divider = "---------------------------------------------";
    private bool _exit = false; // Global flag for exiting


    public async Task Run(string[] args) => await MainMenu();

    private async Task MainMenu()
    {
        bool exitApp = false;

        // Simple menu loop
        while (!exitApp)
        {
            _exit = false;
            Console.Clear();
            Console.WriteLine("Welcome to the Place Order App");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Place an order.");
            Console.WriteLine("2. Order history");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1-3): ");

            switch (Console.ReadLine())
            {
                case "1": await PlaceOrder(); break;
                case "2": await GetOrderHistory(); break;
                case "3": ExitMainMenu(); exitApp = true; break;
                default: Console.WriteLine("Invalid choice. Please select an option between 1 and 3."); break;
            }
        }
    }

    private void ExitMainMenu()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine("Exiting the application...");
    }

    private async Task PlaceOrder()
    {
        var availableProducts = await unitOfWork.ProductRepository.GetProductsAsync();
        var basket = new List<OrderItem>();

        while (!_exit)
        {
            Console.Clear();
            Console.WriteLine("Place Order:");

            availableProducts.ForEach(p => Console.WriteLine($"{p.Id}. {p.Name} | Price: {p.Price} PLN"));

            Console.WriteLine($"{divider}\nOptions:");
            Console.WriteLine("1. Back to main menu");
            Console.WriteLine("2. Add product to basket");
            Console.WriteLine("3. View order summary");
            Console.Write("Enter your choice (1 to go back, 2 to add product, 3 to view summary): ");

            switch (Console.ReadLine())
            {
                case "1": return;
                case "2": AddToBasket(availableProducts, basket); break;
                case "3": await ViewBasket(basket); break;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    private static void AddToBasket(List<Product> availableProducts, List<OrderItem> basket)
    {
        Console.Write("Enter product ID: ");

        if (int.TryParse(Console.ReadLine(), out int productId)
            && availableProducts.FirstOrDefault(p => p.Id == productId) is Product product)
        {
            Console.Write("Enter the quantity: ");

            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                var existingItem = basket.FirstOrDefault(i => i.ProductId == product.Id);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    Console.WriteLine($"Updated {product.Name} quantity to {existingItem.Quantity}.");
                }
                else
                {
                    basket.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = quantity
                    });
                    Console.WriteLine($"Added {quantity} x {product.Name} to the basket.");
                }
            }
            else Console.WriteLine("Invalid quantity. Please enter a positive numeric value.");
        }
        else Console.WriteLine("Invalid input. Please enter a numeric product ID.");

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    private async Task ViewBasket(List<OrderItem> basket)
    {
        while (!_exit)
        {
            Console.Clear();
            Console.WriteLine("Basket Summary:");

            if (basket.Count == 0)
            {
                Console.WriteLine("Your basket is empty.");
                Console.WriteLine("Press any key to return to the place order menu.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < basket.Count; i++)
            {
                var item = basket[i];
                Console.WriteLine($"{i + 1}. {item.ProductName} | Quantity: {item.Quantity} " +
                    $"| Price: {item.Price} PLN | Subtotal: {item.Price * item.Quantity} PLN");
            }

            decimal totalCost = basket.Sum(i => i.Price * i.Quantity);

            Console.WriteLine($"{divider}\nTotal Cost: {totalCost} PLN");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Back to place order menu");
            Console.WriteLine("2. Modify item quantity");
            Console.WriteLine("3. Remove item from basket");
            Console.WriteLine("4. View order summary");
            Console.Write("Enter your choice (1-4): ");

            switch (Console.ReadLine())
            {
                case "1": return;
                case "2": ModifyBasketItem(basket); break;
                case "3": RemoveBasketItem(basket); break;
                case "4": await OrderSummary(basket); break;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    private static void ModifyBasketItem(List<OrderItem> basket)
    {
        Console.Write("Enter the item number to modify quantity: ");

        if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber > 0
            && itemNumber <= basket.Count)
        {
            Console.Write("Enter the new quantity: ");

            if (int.TryParse(Console.ReadLine(), out int newQuantity) && newQuantity > 0)
            {
                basket[itemNumber - 1].Quantity = newQuantity;
                Console.WriteLine("Item quantity updated successfully.");
            }
            else Console.WriteLine("Invalid quantity. Please enter a positive numeric value.");
        }
        else Console.WriteLine("Invalid item number. Please try again.");

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    private static void RemoveBasketItem(List<OrderItem> basket)
    {
        Console.Write("Enter the item number to remove: ");

        if (int.TryParse(Console.ReadLine(), out int removeItemNumber)
            && removeItemNumber > 0 && removeItemNumber <= basket.Count)
        {
            basket.RemoveAt(removeItemNumber - 1);
            Console.WriteLine("Item removed successfully.");
        }
        else Console.WriteLine("Invalid item number. Please try again.");

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    private async Task OrderSummary(List<OrderItem> basket)
    {
        while (!_exit)
        {
            Console.Clear();
            Console.WriteLine("Order Summary:");

            decimal totalCost = basket.Sum(item => item.Price * item.Quantity);

            foreach (var item in basket)
            {
                Console.WriteLine($"{item.ProductName} | Quantity: {item.Quantity} " +
                    $"| Price: {item.Price} PLN | Subtotal: {item.Price * item.Quantity} PLN");
            }

            Console.WriteLine("------------------------------------");
            Console.WriteLine($"Total Cost: {totalCost} PLN");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Back to basket summary");
            Console.WriteLine("2. Place order");
            Console.Write("Enter your choice (1-2): ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1": return;

                case "2":
                    await ConfirmOrder(basket, totalCost);
                    break;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    private async Task ConfirmOrder(List<OrderItem> basket, decimal totalCost)
    {
        Console.WriteLine("Are you sure you want to place the order? (yes/no): ");

        switch (Console.ReadLine()?.Trim().ToLower())
        {
            case "yes":
                Console.Clear();
                // Add order to database
                var order = new Order()
                {
                    OrderItems = basket,
                    Subtotal = totalCost,
                    Total = totalCost
                };
                unitOfWork.OrderRepository.Add(order);
                await unitOfWork.Complete();

                Console.WriteLine("Order placed successfully!");
                Console.WriteLine("Thank you for your purchase. Redirecting to the main menu...");

                basket.Clear();

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();

                _exit = true;
                break;
            case "no":
                Console.WriteLine("Order placement canceled. Returning to order summary...");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Invalid input. Please type 'yes' or 'no'.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                break;
        }
    }

    private async Task GetOrderHistory()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Order History:");

            var orders = await unitOfWork.OrderRepository.GetOrdersAsync();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                Console.WriteLine("Press any key to return to the main menu.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                Console.WriteLine($"{i + 1}. Order Date: {order.OrderDate} | Total: {order.Total} PLN " +
                    $"| Items: {order.OrderItems.Sum(x => x.Quantity)}");
            }

            Console.WriteLine($"{divider}\nOptions:");
            Console.WriteLine("1. Back to main menu");
            Console.WriteLine("2. View order details");
            Console.Write("Enter your choice (1-2): ");

            switch (Console.ReadLine())
            {
                case "1": return;
                case "2":
                    Console.Write("Enter the order number to view details: ");

                    if (int.TryParse(Console.ReadLine(), out int orderNumber) && orderNumber > 0
                        && orderNumber <= orders.Count)
                    {
                        var selectedOrder = orders[orderNumber - 1];
                        ViewOrderDetails(selectedOrder);
                    }
                    else
                    {
                        Console.WriteLine("Invalid order number. Please try again.");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ViewOrderDetails(Order order)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Order Details\nOrder Date: {order.OrderDate}\n{divider}");

            order.OrderItems.ForEach(p => Console.WriteLine($"{p.ProductName} | Quantity: {p.Quantity} " +
                    $"| Price: {p.Price} PLN | Subtotal: {p.Price * p.Quantity} PLN"));

            Console.WriteLine($"{divider}\nSubtotal: {order.Subtotal} PLN");
            Console.WriteLine($"Total: {order.Total} PLN");

            Console.WriteLine("Press any key to return to the order history menu.");
            Console.ReadKey();
            return;
        }
    }
}