using NorthwindOrderSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindOrderSystem.Core.Interfaces
{
    public interface IOrderRepository
    {
        // Crear una nueva orden
        Task AddAsync(Order order);

        // Obtener todas las órdenes (incluyendo detalles y navegación)
        Task<List<Order>> GetAllAsync();

        // Obtener una orden por su ID (incluyendo detalles)
        Task<Order> GetByIdAsync(int orderId);

        // Actualizar una orden existente
        Task UpdateAsync(Order order);

        // Eliminar una orden existente
        Task DeleteAsync(Order order);
    }
}
