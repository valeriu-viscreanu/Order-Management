# Order Management API

A robust ASP.NET Core Web API for managing customer orders and their items using Entity Framework Core.

## Features

- **Guid-Based Identities**: Secure and unique identifiers for all resources.
- **Nested Resource Mapping**: Clean RESTful structure for managing items within orders.
- **Entity Framework Core**: Integrated with In-Memory database for blazing fast demonstrations.
- **Schema Compliant**: Implements `OrderNumber`, `TotalPrice` calculations, and specific response formats.
- **Postman Support**: Includes a pre-configured Postman collection for easy testing.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Running the Application

1. Navigate to the project directory:
   ```bash
   cd OrderManagement
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
3. Access the API at `http://localhost:5260/api/orders`.

## API Endpoints

### Orders
| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/orders` | Retrieve all orders |
| GET | `/api/orders/{id}` | Retrieve a specific order by ID |
| POST | `/api/orders` | Create a new order |
| PUT | `/api/orders/{id}` | Update an existing order |
| DELETE | `/api/orders/{id}` | Delete an order |

### Order Items
| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/orders/{orderId}/items` | Retrieve all items for an order |
| GET | `/api/orders/{orderId}/items/{id}` | Retrieve a specific item by ID |
| POST | `/api/orders/{orderId}/items` | Add a new item to an order |
| PUT | `/api/orders/{orderId}/items/{id}` | Update an existing item |
| DELETE | `/api/orders/{orderId}/items/{id}` | Remove an item from an order |

## Testing

- **HTTP File**: Use `OrderManagement.http` inside VS Code / Visual Studio for integrated testing.
- **Postman**: Import `OrderManagement.postman_collection.json` into Postman. Set the `baseUrl` variable to your running server (default: `http://localhost:5260`).


