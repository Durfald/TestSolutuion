using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSolutuion.Server.Domain.OrderManager;
using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Extensions;

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

        [HttpPut("ConfirmOrder/{id}")]
        [DenyRoles(DefaultStaticData.UserRole)]
        public async Task<IActionResult> ConfirmOrder(Guid id, DateTime shipmentDate)
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
        [DenyRoles(DefaultStaticData.UserRole)]
        public async Task<IActionResult> CloseOrder(Guid id)
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
        public async Task<IActionResult> DeleteOrder(Guid id)
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
        public async Task<IActionResult> CreateOrder(Order order)
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
    }
}
