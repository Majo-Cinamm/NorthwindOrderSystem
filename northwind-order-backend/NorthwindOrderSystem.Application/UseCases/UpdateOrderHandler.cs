using NorthwindOrderSystem.Application.DTOs;
using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Application.UseCases
{
    public class UpdateOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderId = orderDto.OrderId.Value, // Ya debe tener ID para actualizar
                CustomerId = orderDto.CustomerId,
                EmployeeId = orderDto.EmployeeId,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                ShipAddress = orderDto.ShipAddress,
                ShipCity = orderDto.ShipCity,
                ShipRegion = orderDto.ShipRegion,
                ShipPostalCode = orderDto.ShipPostalCode,
                ShipCountry = orderDto.ShipCountry,
                Freight = orderDto.Freight,
                OrderDetails = orderDto.OrderDetails.Select(detailDto => new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    UnitPrice = detailDto.UnitPrice,
                    Discount = 0
                }).ToList()
            };

            await _orderRepository.UpdateAsync(order);
        }
    }
}
