
using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Application.UseCases
{
    public class GetAllOrdersHandler
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<Order>> HandleAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}
