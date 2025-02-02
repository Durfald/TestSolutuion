using Microsoft.EntityFrameworkCore;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.RProduct
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SQLiteDataBaseContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByPriceAndCategoryAsync(double minPrice = 0,
            double maxPrice = double.MaxValue,
            string[]? categoryNames = null)
        {
            var query = _context.Products.AsQueryable();

            query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

            if (categoryNames != null)
            {
                query = query.Where(p => categoryNames.Contains(p.Category));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<(string? Category, int Count)>> GetCategories()
        {
            return await _context.Products
                .GroupBy(p => p.Category)
                .Select(g => new ValueTuple<string?, int>(g.Key, g.Count()))
                .ToListAsync();
        }
    }
}
