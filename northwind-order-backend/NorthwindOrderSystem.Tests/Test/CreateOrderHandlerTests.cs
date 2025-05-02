using Moq;
using NorthwindOrderSystem.Application.DTOs;
using NorthwindOrderSystem.Application.UseCases;
using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Core.Interfaces;

namespace NorthwindOrderSystem.Tests.Test
{
    public class CreateOrderHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnOrderId_WhenOrderIsSaved()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Order>()))
                    .Callback<Order>(o => o.OrderId = 42)
                    .Returns(Task.CompletedTask);

            var handler = new CreateOrderHandler(mockRepo.Object);

            var orderDto = new OrderDto
            {
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = System.DateTime.Now,
                ShipAddress = "123 Street",
                ShipCity = "City",
                ShipCountry = "Country",
                OrderDetails = new List<OrderDetailDto>
                {
                    new OrderDetailDto { ProductId = 1, Quantity = 2, UnitPrice = 10.0M }
                }
            };

            // Act
            var result = await handler.HandleAsync(orderDto);

            // Assert
            Assert.Equal(42, result);
        }
    }
}