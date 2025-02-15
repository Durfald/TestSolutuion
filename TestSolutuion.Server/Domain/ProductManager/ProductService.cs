using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Database.Repository.UnitOfWork;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.ProductManager
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryUnitOfWork _repositoryUnitOfWork;

        public ProductService(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var existproduct = await _repositoryUnitOfWork.Product.FindAsync(
                x => x.Name == product.Name&&
                x.Category==product.Category&&
                x.Price==product.Price);
            if (existproduct != null && existproduct.Count() > 0)
                throw new ArgumentException("Product already exists");
            await _repositoryUnitOfWork.Product.AddAsync(product);
            return product;
        }

        public async Task DeleteProductAsync(string id)
        {
            var deleted = await _repositoryUnitOfWork.Product.DeleteAsync(id);
            if (!deleted)
                throw new ArgumentException("Product not found");
        }

        public async Task DeleteProductsAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                await _repositoryUnitOfWork.Product.DeleteAsync(id);
            }
        }

        public async Task<Product> UpdateProductAsync(string id,Product product)
        {
            var dbproduct =await _repositoryUnitOfWork.Product.GetByIdAsync(id);
            if(dbproduct == null)
                throw new ArgumentException("Product not found");
            dbproduct.Name = product.Name;
            dbproduct.Price = product.Price;
            dbproduct.Category = product.Category;
            await _repositoryUnitOfWork.Product.UpdateAsync(dbproduct);
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repositoryUnitOfWork.Product.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = await _repositoryUnitOfWork.Product.GetByIdAsync(id);
            if (product == null)
                throw new ArgumentException("Product not found");
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductByPriceCategoryAsync(int min, int max, string[]? categories)
        {
            return await _repositoryUnitOfWork.Product.GetProductsByPriceAndCategoryAsync(min, max, categories);
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            var categories = await _repositoryUnitOfWork.Product.GetCategories();
            if (categories == null || categories.Count() == 0)
                categories = [];

            return categories?.Select(c => new CategoryModel
            {
                Name = c.Category,
                Count = c.Count
            }) ?? [];
        }
    }

}
