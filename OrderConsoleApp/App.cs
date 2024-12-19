using OrderConsoleApp.Contracts;
using OrderConsoleApp.Entities;

namespace OrderConsoleApp;

public class App(IUnitOfWork unitOfWork, IDiscountService discountService)
{
    // Divider string for separating sections in console output
    private const string divider = "---------------------------------------------";
    // Global flag for exiting
    private bool _exit = false;

    // Main entry point for the application, runs the main menu
    public async Task Run(string[] args) => await MainMenu();

    private async Task MainMenu()
    {
        bool exitApp = false;

        // Simple loop that keeps the menu running until the user chooses to exit
        while (!exitApp)
        {
            _exit = false;
            Console.Clear(); // Clear the console before showing the menu
            Console.WriteLine("Welcome to the Place Order App");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Place an order.");
            Console.WriteLine("2. Order history");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1-3): ");

            // Switch statement based on user input to navigate through options
            switch (Console.ReadLine())
            {
                case "1": await PlaceOrder(); break;
                case "2": await GetOrderHistory(); break;
                case "3": ExitMainMenu(); exitApp = true; break;
                default: Console.WriteLine("Invalid choice. Please select an option between 1 and 3."); break;
            }
        }
    }

    // Handles exiting the main menu and displaying an exit message
    private void ExitMainMenu()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine("Exiting the application...");
    }

    // Allows the user to place an order by selecting products and quantities
    private async Task PlaceOrder()
    {
        var availableProducts = await unitOfWork.ProductRepository.GetProductsAsync();
        var basket = new List<OrderItem>(); // List to store items added to the basket

        while (!_exit) // Loop until the user decides to exit
        {
            Console.Clear();
            Console.WriteLine("Place Order:");

            // Display available products and their prices
            availableProducts.ForEach(p => Console.WriteLine($"{p.Id}. {p.Name} | Price: {p.Price} PLN"));

            Console.WriteLine($"{divider}\nOptions:");
            Console.WriteLine("1. Back to main menu");
            Console.WriteLine("2. Add product to basket");
            Console.WriteLine("3. View order summary");
            Console.Write("Enter your choice (1 to go back, 2 to add product, 3 to view summary): ");

            // Switch statement for user input
            switch (Console.ReadLine())
            {
                case "1": return;
                case "2": AddToBasket(availableProducts, basket); break;
                case "3": await ViewBasket(basket); break;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    // Adds a product to the basket
    private static void AddToBasket(List<Product> availableProducts, List<OrderItem> basket)
    {
        Console.Write("Enter product ID: ");

        // Attempt to parse user input as a valid product ID
        if (int.TryParse(Console.ReadLine(), out int productId)
            && availableProducts.FirstOrDefault(p => p.Id == productId) is Product product)
        {
            Console.Write("Enter the quantity: ");

            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
            {
                // If the item exists, update the quantity
                var existingItem = basket.FirstOrDefault(i => i.ProductId == product.Id);

                if (existingItem != null)
                {
                    // Otherwise, add a new item to the basket
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

    // Views the current contents of the basket
    private async Task ViewBasket(List<OrderItem> basket)
    {
        while (!_exit)
        {
            Console.Clear();
            Console.WriteLine("Basket Summary:");

            // Check if the basket is empty
            if (basket.Count == 0)
            {
                Console.WriteLine("Your basket is empty.");
                Console.WriteLine("Press any key to return to the place order menu.");
                Console.ReadKey();
                return;
            }

            // Display all items in the basket
            for (int i = 0; i < basket.Count; i++)
            {
                var item = basket[i];
                Console.WriteLine($"{i + 1}. {item.ProductName} | Quantity: {item.Quantity} " +
                    $"| Price: {item.Price} PLN | Subtotal: {item.Price * item.Quantity} PLN");
            }

            // Calculate the total cost of the basket
            decimal totalCost = basket.Sum(i => i.Price * i.Quantity);

            Console.WriteLine($"{divider}\nTotal Cost: {totalCost} PLN");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Back to place order menu");
            Console.WriteLine("2. Modify item quantity");
            Console.WriteLine("3. Remove item from basket");
            Console.WriteLine("4. View order summary");
            Console.Write("Enter your choice (1-4): ");

            // Switch statement for user input
            switch (Console.ReadLine())
            {
                case "1": return;
                case "2": ModifyBasketItem(basket); break;
                case "3": RemoveBasketItem(basket); break;
                case "4": await OrderSummary(basket, totalCost); break;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    // Allows the user to modify the quantity of an item in the basket
    private static void ModifyBasketItem(List<OrderItem> basket)
    {
        Console.Write("Enter the item number to modify quantity: ");

        // Parse user input to get the item number
        if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber > 0
            && itemNumber <= basket.Count)
        {
            Console.Write("Enter the new quantity: ");

            // Parse user input for the new quantity
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

    // Allows the user to remove an item from the basket
    private static void RemoveBasketItem(List<OrderItem> basket)
    {
        Console.Write("Enter the item number to remove: ");

        // Parse user input to get the item number
        if (int.TryParse(Console.ReadLine(), out int removeItemNumber)
            && removeItemNumber > 0 && removeItemNumber <= basket.Count)
        {
            basket.RemoveAt(removeItemNumber - 1); // Remove the item from the basket
            Console.WriteLine("Item removed successfully.");
        }
        else Console.WriteLine("Invalid item number. Please try again.");

        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    // Displays the order summary, including discounts
    private async Task OrderSummary(List<OrderItem> basket, decimal totalCost)
    {
        while (!_exit)
        {
            Console.Clear();
            Console.WriteLine("Order Summary:");

            // Display all items in the basket
            foreach (var item in basket)
            {
                Console.WriteLine($"{item.ProductName} | Quantity: {item.Quantity} " +
                    $"| Price: {item.Price} PLN | Subtotal: {item.Price * item.Quantity} PLN");
            }

            // Create an order object and calculate the discount
            var order = new Order()
            {
                OrderItems = basket,
                Subtotal = totalCost,
            };
            var discount = discountService.GetDiscount(order);

            Console.WriteLine("------------------------------------");
            Console.WriteLine($"Subtotal: {totalCost} PLN");
            Console.WriteLine($"Discount: -{discount} PLN");
            Console.WriteLine($"Total cost: {order.Total} PLN");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Back to basket summary");
            Console.WriteLine("2. Place order");
            Console.Write("Enter your choice (1-2): ");

            // Switch statement for user input
            switch (Console.ReadLine())
            {
                case "1": return;
                case "2": await ConfirmOrder(basket, order); break;
                default: Console.WriteLine("Invalid choice. Press any key to try again."); Console.ReadKey(); break;
            }
        }
    }

    // Confirms and places the order
    private async Task ConfirmOrder(List<OrderItem> basket, Order order)
    {
        Console.WriteLine("Are you sure you want to place the order? (yes/no): ");

        // Switch statement for user input
        switch (Console.ReadLine()?.Trim().ToLower())
        {
            case "yes":
                Console.Clear();
                // Add order to database
                unitOfWork.OrderRepository.Add(order);
                await unitOfWork.Complete();

                Console.WriteLine("Order placed successfully!");
                Console.WriteLine("Thank you for your purchase. Redirecting to the main menu...");

                basket.Clear(); // Clear the basket after order is placed

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();

                _exit = true; // Exit the process
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

    // Displays the order history to the user
    private async Task GetOrderHistory()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Order History:");

            // Retrieve the list of orders from the repository
            var orders = await unitOfWork.OrderRepository.GetOrdersAsync();

            // If no orders are found, display a message
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                Console.WriteLine("Press any key to return to the main menu.");
                Console.ReadKey();
                return;
            }

            // Display the list of orders
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

            // Switch statement for user input
            switch (Console.ReadLine())
            {
                case "1": return; // Go back to the main menu
                case "2":
                    Console.Write("Enter the order number to view details: ");

                    // Parse user input for the order number
                    if (int.TryParse(Console.ReadLine(), out int orderNumber) && orderNumber > 0
                        && orderNumber <= orders.Count)
                    {
                        var selectedOrder = orders[orderNumber - 1];
                        ViewOrderDetails(selectedOrder); // View the details of the selected order
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

    // Displays the details of a selected order
    private static void ViewOrderDetails(Order order)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Order Details\nOrder Date: {order.OrderDate:dd MMM yyyy hh:mm}\n{divider}");

            // Display each item in the order
            order.OrderItems.ForEach(p => Console.WriteLine($"{p.ProductName} | Quantity: {p.Quantity} " +
                    $"| Price: {p.Price} PLN | Subtotal: {p.Price * p.Quantity} PLN"));

            // Display the order's subtotal, discount, and total cost
            Console.WriteLine($"{divider}\nSubtotal: {order.Subtotal} PLN");
            Console.WriteLine($"Discount: -{order.Subtotal - order.Total} PLN");
            Console.WriteLine($"Total: {order.Total} PLN");

            Console.WriteLine("Press any key to return to the order history menu.");
            Console.ReadKey();
            return;
        }
    }
}