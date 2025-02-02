using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.RProduct
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Получить товары по цене
        /// </summary>
        /// <param name="MinPrice"> Минимальная цена</param>
        /// <param name="MaxPrice"> Максимальная цена</param>
        /// <param name="CategoryNames"> Имя категориий</param>
        /// <returns></returns>
        public Task<IEnumerable<Product>> GetProductsByPriceAndCategoryAsync(
            double MinPrice = 0,
            double MaxPrice = double.MaxValue,
            string[]? CategoryNames = null);

        public Task<IEnumerable<(string? Category, int Count)>> GetCategories();
    }
}
