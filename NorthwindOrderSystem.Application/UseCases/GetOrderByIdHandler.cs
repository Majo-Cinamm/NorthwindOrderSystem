using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Application.UseCases
{
    public class GetOrderByIdHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> HandleAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }
    }
}
