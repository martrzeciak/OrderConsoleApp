using OrderConsoleApp.Contracts;

namespace OrderConsoleApp;

public class App(IProductRepository productRepository)
{
    public async Task Run(string[] args)
    {
        bool exit = false;

        // Simple menu loop
        while (!exit)
        {
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
                    Console.WriteLine("Place an order here");
                    break;

                case "2":
                    Console.WriteLine("Order history");
                    break;

                case "3":
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    exit = true;
                    Console.WriteLine("Exiting the application...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select an option between 1 and 3.");
                    break;
            }
        }
    }
}