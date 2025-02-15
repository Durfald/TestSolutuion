using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSolutuion.Server.Domain.CustomerService;
using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Extensions;

namespace TestSolutuion.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowRoles(DefaultStaticData.ManagerRole)]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet("GetCustomer/{id}")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error get customer");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get customer");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error get customers");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get customers");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error delete customer");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error delete customer");
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("UpdateCustomer/{id}")]
        public async Task<IActionResult> UpdateCustomer(string id,[FromBody] Customer customer)
        {
            try
            {
                var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customer);
                return Ok(updatedCustomer);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error update customer");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error update customer");
                return BadRequest(ex.Message);
            }
        }

    }
}