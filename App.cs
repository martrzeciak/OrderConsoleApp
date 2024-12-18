using OrderConsoleApp.Contracts;
using OrderConsoleApp.Entities;

namespace OrderConsoleApp;

public class App(IProductRepository productRepository)
{
    private bool exit = false; // Global flag for exiting

    public async Task Run(string[] args)
    {
        await MainMenu();
    }

    public async Task MainMenu()
    {
        bool exitApp = false;
        // Simple menu loop
        while (!exitApp)
        {
            exit = false;
            Console.Clear();
            Console.WriteLine("Welcome to the Place Order App");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Place an order.");
            Console.WriteLine("2. Order history");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1-3): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await PlaceOrder();
                    break;

                case "2":
                    Console.WriteLine("Order history");
                    break;

                case "3":
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    exitApp = true;
                    Console.WriteLine("Exiting the application...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select an option between 1 and 3.");
                    break;
            }
        }
    }

    private async Task PlaceOrder()
    {
        var availableProducts = await productRepository.GetProductsAsync();
        var basket = new List<OrderItem>();

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("\nPlace order:");

            foreach (var product in availableProducts)
            {
                Console.WriteLine($"{product.Id}. {product.Name} | Price: {product.Price} PLN");
            }

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Back to main menu");
            Console.WriteLine("2. Add product to basket");
            Console.WriteLine("3. View order summary");
            Console.Write("Enter your choice (1 to go back, 2 to add product, 3 to view summary): ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    return;

                case "2":
                    Console.Write("Enter the product ID to add to the basket: ");
                    if (int.TryParse(Console.ReadLine(), out int productId))
                    {
                        var selectedProduct = availableProducts.FirstOrDefault(p => p.Id == productId);

                        if (selectedProduct == null)
                        {
                            Console.WriteLine("Product not found. Please enter a valid product ID.");
                            break;
                        }

                        Console.Write("Enter the quantity: ");
                        if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                        {
                            basket.Add(new OrderItem
                            {
                                ProductId = selectedProduct.Id,
                                ProductName = selectedProduct.Name,
                                Price = selectedProduct.Price,
                                Quantity = quantity
                            });
                            Console.WriteLine($"Added {quantity} x {selectedProduct.Name} to the basket.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity. Please enter a positive numeric value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a numeric product ID.");
                    }

                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;

                case "3":
                    await BasketSummary(basket);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task BasketSummary(List<OrderItem> basket)
    {
        while (!exit)
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

            decimal totalCost = 0;

            for (int i = 0; i < basket.Count; i++)
            {
                var item = basket[i];
                totalCost += item.Price * item.Quantity;
                Console.WriteLine($"{i + 1}. {item.ProductName} | Quantity: {item.Quantity} " +
                    $"| Price: {item.Price} PLN | Subtotal: {item.Price * item.Quantity} PLN");
            }

            Console.WriteLine("------------------------------------");
            Console.WriteLine($"Total Cost: {totalCost} PLN");
            Console.WriteLine("Options:");
            Console.WriteLine("1. Back to place order menu");
            Console.WriteLine("2. Modify item quantity");
            Console.WriteLine("3. Remove item from basket");
            Console.WriteLine("4. View order summary");
            Console.Write("Enter your choice (1-4): ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    return;

                case "2":
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
                        else
                        {
                            Console.WriteLine("Invalid quantity. Please enter a positive numeric value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid item number. Please try again.");
                    }

                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;

                case "3":
                    Console.Write("Enter the item number to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int removeItemNumber)
                        && removeItemNumber > 0 && removeItemNumber <= basket.Count)
                    {
                        basket.RemoveAt(removeItemNumber - 1);
                        Console.WriteLine("Item removed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid item number. Please try again.");
                    }

                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    break;

                case "4":
                    await OrderSummary(basket);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task OrderSummary(List<OrderItem> basket)
    {
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("\nOrder Summary:");

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
                case "1":
                    return;

                case "2":
                    Console.WriteLine("Are you sure you want to place the order? (yes/no): ");
                    var confirmation = Console.ReadLine()?.Trim().ToLower();

                    if (confirmation == "yes")
                    {
                        Console.Clear();
                        Console.WriteLine("Order placed successfully!");
                        Console.WriteLine("Thank you for your purchase. Redirecting to the main menu...");
                        basket.Clear();
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                        exit = true;
                    }
                    else if (confirmation == "no")
                    {
                        Console.WriteLine("Order placement canceled. Returning to order summary...");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please type 'yes' or 'no'.");
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

}