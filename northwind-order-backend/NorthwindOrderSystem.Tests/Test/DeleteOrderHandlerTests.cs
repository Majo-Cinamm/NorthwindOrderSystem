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
    public class DeleteOrderHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCallDeleteAsync_WithCorrectId()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();
            var handler = new DeleteOrderHandler(mockRepo.Object);

            // Act
            await handler.HandleAsync(5);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(It.Is<Order>(o => o.OrderId == 5)), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_ShouldNotThrow_WhenIdDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IOrderRepository>();
            mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            var handler = new DeleteOrderHandler(mockRepo.Object);

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => handler.HandleAsync(999));
            Assert.Null(exception);
            mockRepo.Verify(r => r.DeleteAsync(It.Is<Order>(o => o.OrderId == 999)), Times.Once);
        }
    }
}
