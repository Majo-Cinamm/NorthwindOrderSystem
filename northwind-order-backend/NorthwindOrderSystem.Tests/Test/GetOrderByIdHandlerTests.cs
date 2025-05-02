using Moq;
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
    public class GetOrderByIdHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldReturnOrder_WhenIdIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();
            var expectedOrder = new Order { OrderId = 1, CustomerId = "ALFKI" };

            mockRepo.Setup(r => r.GetByIdAsync(1))
                    .ReturnsAsync(expectedOrder);

            var handler = new GetOrderByIdHandler(mockRepo.Object);

            // Act
            var result = await handler.HandleAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ALFKI", result.CustomerId);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(99))
                    .ReturnsAsync((Order)null);

            var handler = new GetOrderByIdHandler(mockRepo.Object);

            // Act
            var result = await handler.HandleAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnOrderWithMultipleDetails()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();

            var expectedOrder = new Order
            {
                OrderId = 2,
                CustomerId = "BLAUS",
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, Quantity = 2, UnitPrice = 10.0m },
                    new OrderDetail { ProductId = 2, Quantity = 1, UnitPrice = 20.0m }
                }
            };

            mockRepo.Setup(r => r.GetByIdAsync(2))
                    .ReturnsAsync(expectedOrder);

            var handler = new GetOrderByIdHandler(mockRepo.Object);

            // Act
            var result = await handler.HandleAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.OrderDetails.Count);
            Assert.Contains(result.OrderDetails, d => d.ProductId == 1);
            Assert.Contains(result.OrderDetails, d => d.ProductId == 2);
        }
    }
}
