using Microsoft.AspNetCore.Mvc;
using NorthwindOrderSystem.Application.DTOs;
using NorthwindOrderSystem.Application.UseCases;
using NorthwindOrderSystem.Core.Interfaces;
using NorthwindOrderSystem.Infrastructure.Repositories;
using NorthwindOrderSystem.Infrastructure.Services;

namespace NorthwindOrderSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        
        private readonly CreateOrderHandler _createOrderHandler;
        private readonly GetOrderByIdHandler _getOrderByIdHandler;
        private readonly GetAllOrdersHandler _getAllOrdersHandler;
        private readonly UpdateOrderHandler _updateOrderHandler;
        private readonly DeleteOrderHandler _deleteOrderHandler;
        private readonly IOrderRepository _orderRepository;


        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;

            _createOrderHandler = new CreateOrderHandler(orderRepository);
            _getOrderByIdHandler = new GetOrderByIdHandler(orderRepository);
            _getAllOrdersHandler = new GetAllOrdersHandler(orderRepository);
            _updateOrderHandler = new UpdateOrderHandler(orderRepository);
            _deleteOrderHandler = new DeleteOrderHandler(orderRepository);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
                return BadRequest("Order data is required.");

            var createdOrderId = await _createOrderHandler.HandleAsync(orderDto);

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrderId }, new { OrderId = createdOrderId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _getOrderByIdHandler.HandleAsync(id);
            if (order == null)
                return NotFound();

            var dto = new OrderDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                EmployeeId = order.EmployeeId,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                RequiredDate = order.RequiredDate,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,
                Freight = order.Freight,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity ?? 0,
                    UnitPrice = od.UnitPrice,
                    ProductName = od.Product?.ProductName
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _getAllOrdersHandler.HandleAsync();

            var result = orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                EmployeeId = order.EmployeeId,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                RequiredDate = order.RequiredDate,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry,
                Freight = order.Freight,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity ?? 0,
                    UnitPrice = od.UnitPrice,
                    ProductName = od.Product?.ProductName
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            if (orderDto == null || orderDto.OrderId != id)
                return BadRequest("Invalid order data.");

            await _updateOrderHandler.HandleAsync(orderDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _deleteOrderHandler.HandleAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> DownloadOrderPdf(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(); // 👈 MUY IMPORTANTE
            }

            // 🔍 Validaciones extras antes de generar PDF
            if (order.OrderDetails == null)
                return BadRequest("La orden no tiene detalles cargados.");

            if (order.Customer == null)
                return BadRequest("La orden no tiene cliente cargado.");

            if (order.Employee == null)
                return BadRequest("La orden no tiene empleado cargado.");

            var pdfService = new OrderPdfService();

            try
            {
                var pdfBytes = pdfService.GenerateOrderPdf(order);
                return File(pdfBytes, "application/pdf", $"Order_{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generando PDF: {ex.Message}");
            }
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> DownloadAllOrdersPdf()
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders == null || !orders.Any())
                return NotFound("No hay órdenes disponibles para exportar.");

            var pdfService = new OrderPdfService();
            var pdfBytes = pdfService.GenerateAllOrdersPdf(orders.ToList());

            return File(pdfBytes, "application/pdf", "AllOrders.pdf");
        }

    }
}

