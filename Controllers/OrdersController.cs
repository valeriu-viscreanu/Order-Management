using Microsoft.AspNetCore.Mvc;
using OrderManagement.Models;

namespace OrderManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> _orders = new()
    {
        new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            OrderDate = DateTime.Now.AddDays(-1),
            TotalAmount = 150.00m,
            Items = new List<OrderItem>
            {
                new OrderItem { Id = 1, ProductName = "Laptop", Quantity = 1, UnitPrice = 1000.00m },
                new OrderItem { Id = 2, ProductName = "Mouse", Quantity = 1, UnitPrice = 50.00m }
            }
        },
        new Order
        {
            Id = 2,
            CustomerName = "Jane Smith",
            OrderDate = DateTime.Now,
            TotalAmount = 250.00m,
            Items = new List<OrderItem>
            {
                new OrderItem { Id = 3, ProductName = "Monitor", Quantity = 1, UnitPrice = 250.00m }
            }
        }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        return Ok(_orders);
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost]
    public ActionResult<Order> CreateOrder(Order order)
    {
        order.Id = _orders.Max(o => o.Id) + 1;
        _orders.Add(order);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOrder(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        _orders.Remove(order);
        return NoContent();
    }
}
