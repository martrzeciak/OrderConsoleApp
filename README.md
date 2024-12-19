# OrderConsoleApp

This is a command-line application that simulates an order placement system. The application allows users to browse products, 
add them to their basket, apply discounts, and place orders. It also provides features for viewing order history and details. 
The app interacts with a SQLite database.

## Features
* Place an Order:
  * Browse available products with their prices.
  * Add products to the basket and specify the quantity.
  * Modify item quantities or remove items from the basket.
  * View an order summary with discounts applied.
  * Confirm the order and save it to the database.
* Order History:
  * View a list of past orders with details such as order date, total cost, and the number of items.
  * View the details of each order, including product names, quantities, prices, and the total cost after discounts.
* Discount Calculation:
  * Apply discounts based on the order's total price.
  * Apply additional discounts when all items are single-quantity items.
  * A 5% discount is applied for orders exceeding a specific price threshold.
  * Extra discounts of 10% and 20% are applied to the second and third most expensive items when all items are single-quantity.

## Used Technologies 
* **Entity Framework Core**: Used for ORM to manage the SQLite database.
* **SQLite**: Database provider for storing products, orders, and order items.
* **Dependency Injection**: Managed by the built-in Microsoft.Extensions.DependencyInjection for better separation of concerns and testability.

## Prerequisites
* .NET SDK (.NET 8.0)
* SQLite

## Running the Application
 1. Clone the repository.
 2. Restore dependencies:
  ```bash
  dotnet restore
  ```
 3. Run the application:
  ```bash
  ddotnet run
  ```
4. The application will automatically handle the database migration when it starts.
