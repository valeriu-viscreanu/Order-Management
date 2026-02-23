using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Controllers;
using OrderManagement.Data;
using OrderManagement.Models;
using Xunit;

namespace OrderManagement.Tests;

public class OrdersControllerTests
{
    private OrderDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }

    [Fact]
    public async Task GetOrders_ReturnsAllOrders()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);

        // Act
        var result = await controller.GetOrders();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Order>>>(result);
        var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(actionResult.Value);
        Assert.NotEmpty(orders); // Seeding happens in constructor
    }

    [Fact]
    public async Task GetOrder_ExistingId_ReturnsOrder()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);
        var seededOrders = await context.Orders.ToListAsync();
        var targetId = seededOrders.First().OrderId;

        // Act
        var result = await controller.GetOrder(targetId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var order = Assert.IsType<Order>(okResult.Value);
        Assert.Equal(targetId, order.OrderId);
    }

    [Fact]
    public async Task GetOrder_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);

        // Act
        var result = await controller.GetOrder(Guid.NewGuid());

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task CreateOrder_ValidOrder_ReturnsCreatedResponse()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);
        var newOrder = new Order
        {
            OrderNumber = "ORD999",
            CustomerName = "New Customer",
            OrderDate = DateTime.UtcNow,
            TotalAmount = 100.0m,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductName = "New Product", Quantity = 1, UnitPrice = 100.0m }
            }
        };

        // Act
        var result = await controller.CreateOrder(newOrder);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnedOrder = Assert.IsType<Order>(createdResult.Value);
        Assert.Equal(newOrder.OrderNumber, returnedOrder.OrderNumber);
        
        // Verify database
        Assert.NotNull(await context.Orders.FindAsync(returnedOrder.OrderId));
    }

    [Fact]
    public async Task UpdateOrder_ValidOrder_UpdatesAndReturnsOk()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);
        var seededOrders = await context.Orders.ToListAsync();
        var orderToUpdate = seededOrders.First();
        orderToUpdate.CustomerName = "Updated Name";

        // Act
        var result = await controller.UpdateOrder(orderToUpdate.OrderId, orderToUpdate);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrder = Assert.IsType<Order>(okResult.Value);
        Assert.Equal("Updated Name", returnedOrder.CustomerName);
    }

    [Fact]
    public async Task DeleteOrder_ExistingId_RemovesOrder()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);
        var seededOrders = await context.Orders.ToListAsync();
        var targetId = seededOrders.First().OrderId;

        // Act
        var result = await controller.DeleteOrder(targetId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(await context.Orders.FindAsync(targetId));
    }

    [Fact]
    public async Task GetOrderItems_ReturnsItemsForOrder()
    {
        // Arrange
        using var context = GetDbContext();
        var controller = new OrdersController(context);
        var seededOrders = await context.Orders.Include(o => o.Items).ToListAsync();
        var targetOrder = seededOrders.First();

        // Act
        var result = await controller.GetOrderItems(targetOrder.OrderId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<OrderItem>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<OrderItem>>(okResult.Value);
        Assert.Equal(targetOrder.Items.Count, items.Count());
    }
}
