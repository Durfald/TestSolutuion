using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.ProductManager
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
        Task DeleteProductsAsync(IEnumerable<Guid> ids);
        Task<Product> UpdateProductAsync(Guid id,Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetProductByPriceCategoryAsync(int min, int max, string[]? categories);
        Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
    }
}
