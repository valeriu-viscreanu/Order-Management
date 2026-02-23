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
        
        // Seed database if empty
        if (!_context.Orders.Any())
        {
            _context.Orders.AddRange(
                new Order
                {
                    CustomerName = "John Doe",
                    OrderDate = DateTime.Now.AddDays(-1),
                    TotalAmount = 1050.00m,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ProductName = "Laptop", Quantity = 1, UnitPrice = 1000.00m },
                        new OrderItem { ProductName = "Mouse", Quantity = 1, UnitPrice = 50.00m }
                    }
                },
                new Order
                {
                    CustomerName = "Jane Smith",
                    OrderDate = DateTime.Now,
                    TotalAmount = 250.00m,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ProductName = "Monitor", Quantity = 1, UnitPrice = 250.00m }
                    }
                }
            );
            _context.SaveChanges();
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders.Include(o => o.Items).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
