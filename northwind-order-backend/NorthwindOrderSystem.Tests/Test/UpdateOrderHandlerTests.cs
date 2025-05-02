using Moq;
using NorthwindOrderSystem.Application.DTOs;
using NorthwindOrderSystem.Application.UseCases;
using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Tests.Test
{
    public class UpdateOrderHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCallUpdateAsync_WithMappedOrder()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();

            var orderDto = new OrderDto
            {
                OrderId = 100,
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = System.DateTime.Now,
                ShipAddress = "456 Updated St",
                ShipCity = "UpdatedCity",
                ShipCountry = "UpdatedCountry",
                OrderDetails = new List<OrderDetailDto>
                {
                    new OrderDetailDto { ProductId = 2, Quantity = 3, UnitPrice = 15.0M }
                }
            };

            var handler = new UpdateOrderHandler(mockRepo.Object);

            // Act
            await handler.HandleAsync(orderDto);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Order>(o =>
                o.OrderId == 100 &&
                o.CustomerId == "ALFKI" &&
                o.EmployeeId == 1 &&
                o.ShipAddress == "456 Updated St" &&
                o.OrderDetails.Count == 1 &&
                o.OrderDetails.First().ProductId == 2
            )), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldNotCallUpdate_WhenOrderDtoIsNull()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();
            var handler = new UpdateOrderHandler(mockRepo.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.HandleAsync(null));
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
        }
    }
}
