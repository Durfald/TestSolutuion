using Microsoft.AspNetCore.Mvc;
using TestSolutuion.Server.Domain.ProductManager;
using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Extensions;

namespace TestSolutuion.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost("CreateProduct")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                return Ok(createdProduct);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error create user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error delete user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error delete user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteProducts")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> DeleteProducts(IEnumerable<string> ids)
        {
            await _productService.DeleteProductsAsync(ids);
            return Ok();
        }

        [HttpPut("UpdateProduct/{id}")]
        [AllowRoles(DefaultStaticData.ManagerRole)]
        public async Task<IActionResult> UpdateProduct(string id,Product product)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, product);
                return Ok(updatedProduct);
            }
            catch (ArgumentException ex) {
                _logger.LogError(ex, "Error update user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error update user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error get user");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error get user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        public record struct PriceCategory(int min, int max, string[]? categories);
        [HttpGet("GetProductByPriceCategory")]
        public async Task<IActionResult> GetProductByPriceCategory(PriceCategory priceCategory)
        {
            var products = await _productService.GetProductByPriceCategoryAsync(priceCategory.min, priceCategory.max, priceCategory.categories);
            return Ok(products);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }

}
