using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Models;

namespace OrderManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _context;

    public OrdersController(OrderDbContext context)
    {
        _context = context;
        SeedData();
    }

    private void SeedData()
    {
        if (!_context.Orders.Any())
        {
            var orderId1 = Guid.NewGuid();
            var orderId2 = Guid.NewGuid();

            _context.Orders.AddRange(
                new Order
                {
                    OrderId = orderId1,
                    OrderNumber = "ORD001",
                    CustomerName = "John Doe",
                    OrderDate = DateTime.SpecifyKind(DateTime.Parse("2023-07-14T10:30:00"), DateTimeKind.Utc),
                    TotalAmount = 66.5m,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { OrderItemId = Guid.NewGuid(), OrderId = orderId1, ProductName = "Product A", Quantity = 2, UnitPrice = 10.0m },
                        new OrderItem { OrderItemId = Guid.NewGuid(), OrderId = orderId1, ProductName = "Product B", Quantity = 3, UnitPrice = 15.5m }
                    }
                },
                new Order
                {
                    OrderId = orderId2,
                    OrderNumber = "ORD002",
                    CustomerName = "Jane Smith",
                    OrderDate = DateTime.SpecifyKind(DateTime.Parse("2023-07-14T11:45:00"), DateTimeKind.Utc),
                    TotalAmount = 225.8m,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { OrderItemId = Guid.NewGuid(), OrderId = orderId2, ProductName = "Product X", Quantity = 1, UnitPrice = 225.8m }
                    }
                }
            );
            _context.SaveChanges();
        }
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.Include(o => o.Items).ToListAsync();
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
        var order = await _context.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();
        return Ok(order);
    }

    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        if (order.OrderId == Guid.Empty) order.OrderId = Guid.NewGuid();
        foreach (var item in order.Items)
        {
            if (item.OrderItemId == Guid.Empty) item.OrderItemId = Guid.NewGuid();
            item.OrderId = order.OrderId;
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
    }

    // PUT: api/orders/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, Order order)
    {
        var existingOrder = await _context.Orders.FindAsync(id);
        if (existingOrder == null) return NotFound();

        existingOrder.OrderNumber = order.OrderNumber;
        existingOrder.CustomerName = order.CustomerName;
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.TotalAmount = order.TotalAmount;

        await _context.SaveChangesAsync();
        return Ok(existingOrder);
    }

    // DELETE: api/orders/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // --- ORDER ITEMS ENDPOINTS ---

    // GET: api/orders/{orderId}/items
    [HttpGet("{orderId}/items")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(Guid orderId)
    {
        var order = await _context.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null) return NotFound();
        return Ok(order.Items);
    }

    // GET: api/orders/{orderId}/items/{id}
    [HttpGet("{orderId}/items/{id}")]
    public async Task<ActionResult<OrderItem>> GetOrderItem(Guid orderId, Guid id)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(i => i.OrderId == orderId && i.OrderItemId == id);

        if (item == null) return NotFound();
        return Ok(item);
    }

    // POST: api/orders/{orderId}/items
    [HttpPost("{orderId}/items")]
    public async Task<ActionResult<OrderItem>> AddOrderItem(Guid orderId, OrderItem item)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        item.OrderId = orderId;
        if (item.OrderItemId == Guid.Empty) item.OrderItemId = Guid.NewGuid();

        _context.OrderItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderItem), new { orderId = orderId, id = item.OrderItemId }, item);
    }

    // PUT: api/orders/{orderId}/items/{id}
    [HttpPut("{orderId}/items/{id}")]
    public async Task<IActionResult> UpdateOrderItem(Guid orderId, Guid id, OrderItem item)
    {
        var existingItem = await _context.OrderItems
            .FirstOrDefaultAsync(i => i.OrderId == orderId && i.OrderItemId == id);

        if (existingItem == null) return NotFound();

        existingItem.ProductName = item.ProductName;
        existingItem.Quantity = item.Quantity;
        existingItem.UnitPrice = item.UnitPrice;

        await _context.SaveChangesAsync();
        return Ok(existingItem);
    }

    // DELETE: api/orders/{orderId}/items/{id}
    [HttpDelete("{orderId}/items/{id}")]
    public async Task<IActionResult> DeleteOrderItem(Guid orderId, Guid id)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(i => i.OrderId == orderId && i.OrderItemId == id);

        if (item == null) return NotFound();

        _context.OrderItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
