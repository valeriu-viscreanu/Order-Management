# OrderManagement API

A high-performance ASP.NET Core Web API for managing customer orders.

## Features

- **Standard RESTful Endpoints**: Clean and intuitive API design.
- **In-Memory Storage**: Quick setup for demonstration purposes.
- **Strongly Typed Models**: Robust data handling with .NET 10.
- **OpenAPI Integration**: Built-in documentation ready for Swagger/OpenAPI.

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

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/orders` | Retrieve all orders |
| GET | `/api/orders/{id}` | Retrieve a specific order by ID |
| POST | `/api/orders` | Create a new order |
| DELETE | `/api/orders/{id}` | Delete an order |

## Design Decisions

- **Controller-based approach**: Chosen for its familiarity and robust feature set for enterprise APIs.
- **Clean Architecture Principles**: Models and Controllers are separated for better maintainability.
- **Modern .NET Features**: Utilizes file-scoped namespaces and primary constructors where applicable.

---
Developed with ❤️ by Antigravity.
