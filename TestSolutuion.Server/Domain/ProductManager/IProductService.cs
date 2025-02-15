using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.ProductManager
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task DeleteProductAsync(string id);
        Task DeleteProductsAsync(IEnumerable<string> ids);
        Task<Product> UpdateProductAsync(string id,Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<IEnumerable<Product>> GetProductByPriceCategoryAsync(int min, int max, string[]? categories);
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
    }
}
