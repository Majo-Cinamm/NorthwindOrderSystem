using NorthwindOrderSystem.Core.Entities;
using NorthwindOrderSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindOrderSystem.Core.Interfaces;

namespace NorthwindOrderSystem.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly NorthwindDbContext _context;

        public OrderRepository(NorthwindDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                
                 .AsNoTracking() // 👈 Agrega esto para evitar errores por tracking incompleto

                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateAsync(Order order)
        {
            // Primero obtenemos la orden existente con sus detalles
            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (existingOrder != null)
            {
                // Actualizamos las propiedades básicas
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.EmployeeId = order.EmployeeId;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.RequiredDate = order.RequiredDate;
                existingOrder.ShippedDate = order.ShippedDate;
                existingOrder.ShipAddress = order.ShipAddress;
                existingOrder.ShipCity = order.ShipCity;
                existingOrder.ShipRegion = order.ShipRegion;
                existingOrder.ShipPostalCode = order.ShipPostalCode;
                existingOrder.ShipCountry = order.ShipCountry;
                existingOrder.Freight = order.Freight;

                // Actualizamos los detalles de la orden:
                // 1. Eliminamos los detalles actuales
                _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);

                // 2. Agregamos los nuevos detalles
                existingOrder.OrderDetails = order.OrderDetails;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Order order)
        {
            // Cargamos la orden con detalles
            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            if (existingOrder != null)
            {
                // Primero eliminamos sus detalles (dependencias)
                _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);

                // Luego eliminamos la orden
                _context.Orders.Remove(existingOrder);

                await _context.SaveChangesAsync();
            }
        }
    }
}

