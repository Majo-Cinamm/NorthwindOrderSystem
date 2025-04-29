using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindOrderSystem.Application.DTOs;
using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Core.Interfaces;

namespace NorthwindOrderSystem.Application.UseCases
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> HandleAsync(OrderDto orderDto)
        {
            // Crear entidad Order
            var order = new Order
            {
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
                OrderDetails = new List<OrderDetail>()
            };

            // Crear cada OrderDetail
            foreach (var detailDto in orderDto.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    UnitPrice = detailDto.UnitPrice,
                    Discount = 0 // Default, podrías cambiarlo si quieres permitir descuentos
                };

                order.OrderDetails.Add(orderDetail);
            }

            // Guardar en la base de datos
            await _orderRepository.AddAsync(order);

            // Retornar el ID de la orden creada
            return order.OrderId;
        }
    }
}

