using OrderManagement.Models;
using Xunit;

namespace OrderManagement.Tests;

public class ModelsTests
{
    [Fact]
    public void OrderItem_TotalPrice_CalculatesCorrectly()
    {
        // Arrange
        var item = new OrderItem
        {
            Quantity = 5,
            UnitPrice = 10.5m
        };

        // Act
        var totalPrice = item.TotalPrice;

        // Assert
        Assert.Equal(52.5m, totalPrice);
    }
}
