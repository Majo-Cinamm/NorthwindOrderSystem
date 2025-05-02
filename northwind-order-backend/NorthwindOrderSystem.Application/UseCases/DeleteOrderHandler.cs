using NorthwindOrderSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Application.UseCases
{
    public class DeleteOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order != null)
            {
                await _orderRepository.DeleteAsync(order);
            }
        }
    }
}
