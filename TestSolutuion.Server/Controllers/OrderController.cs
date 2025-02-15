using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSolutuion.Server.Domain.OrderManager;
using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Extensions;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet("GetOrders")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> GetHistoryOrders()
        {
            var orders = await _orderService.get();
            return Ok(orders);
        }

        [HttpPut("ConfirmOrder/{id}")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> ConfirmOrder(string id, DateTime shipmentDate)
        {
            try
            {
                await _orderService.ConfirmOrderAsync(id, shipmentDate);
                return Ok();
            }
            catch (ArgumentException ex) 
            { 
                _logger.LogError(ex, "Error confirm order");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirm order");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpPut("CloseOrder/{id}")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> CloseOrder(string id)
        {
            try
            {
                await _orderService.CloseOrderAsync(id);
                return Ok();
            }
            catch (ArgumentException ex) {
                _logger.LogError(ex, "Error close order");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error close order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        [AllowRoles(DefaultStaticData.UserRole)]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return Ok();
            }
            catch (ArgumentException ex) {
                _logger.LogError(ex, "Error delete order");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error delete order");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpPost("CreateOrder")]
        [AllowRoles(DefaultStaticData.UserRole)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel order)
        {
            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(order);
                return Ok(createdOrder);
            }
            catch (ArgumentException ex) {
                _logger.LogError(ex, "Error create order");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetHistory/{id}")]
        public async Task<IActionResult> GetOrders(string id)
        {
            try
            {
                var orders = await _orderService.GetHistoryOrderAsync(id);
                return Ok(orders);
            }
            catch (ArgumentException ex) { 
                _logger.LogError(ex, "Error get orders");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get orders");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
