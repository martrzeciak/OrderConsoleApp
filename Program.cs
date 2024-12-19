using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderConsoleApp;
using OrderConsoleApp.Contracts;
using OrderConsoleApp.Data;
using OrderConsoleApp.Services;

// Create the host for dependency injection and application lifecycle management
using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>(); // Get the logger for logging errors

try
{
    // Handle database migration to apply changes to the database schema
    var dbContext = services.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}
catch (Exception ex)
{
    // Log an error if migration fails and exit the application
    logger.LogError(ex, "An error occurred during database migration.");
    return; // Exit early if migration fails
}

try
{
    await services.GetRequiredService<App>().Run(args); // Run the main application logic
}
catch (Exception ex)
{
    logger.LogError(ex, "An unhandled exception occurred."); // Log any unhandled exceptions
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            // Configure Entity Framework Core with SQLite as the database provider
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=OrderAppDB.db"));

            // Register application services and dependencies
            services.AddScoped<App>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }).ConfigureLogging((_, logging) =>
        {
            // Disable EF logs
            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
            logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
        });
}