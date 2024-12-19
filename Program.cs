using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderConsoleApp;
using OrderConsoleApp.Contracts;
using OrderConsoleApp.Data;

using IHost host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    // Handle database migration separately
    var dbContext = services.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during database migration.");
    return; // Exit early if migration fails
}

try
{
    await services.GetRequiredService<App>().Run(args);
}
catch (Exception ex)
{
    logger.LogError(ex, "An unhandled exception occurred.");
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=OrderAppDB.db"));

            services.AddScoped<App>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }).ConfigureLogging((_, logging) =>
        {
            // Disable EF logs
            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
            logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
        });
}