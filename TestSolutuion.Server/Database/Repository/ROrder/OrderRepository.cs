using Microsoft.EntityFrameworkCore;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.ROrder
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(SQLiteDataBaseContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersWithinDateRangeAsync(
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? shippedBefore = null,
            DateTime? shippedAfter = null)
        {
            var query = _context.Orders.AsQueryable();

            if (createdBefore.HasValue)
                query = query.Where(o => o.OrderDate < createdBefore.Value);

            if (createdAfter.HasValue)
                query = query.Where(o => o.OrderDate > createdAfter.Value);

            if (shippedBefore.HasValue)
                query = query.Where(o => o.ShipmentDate < shippedBefore.Value);

            if (shippedAfter.HasValue)
                query = query.Where(o => o.ShipmentDate > shippedAfter.Value);

            return await query.ToListAsync();
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerAsync(Guid customerId)
        {
            return FindAsync(o => o.CustomerId == customerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders.Where(o => o.Status == status).ToListAsync();
        }

        public async Task<Order?> GetOrderByOrderNumberAsync(int orderNumber)
        {
            return (await FindAsync(o => o.OrderNumber == orderNumber)).FirstOrDefault();
        }

        public async Task<Order?> GetOrderWithOrderElementsAsync(Guid orderId)
        {
            return await _context.Orders
               .Where(o => o.Id == orderId)
               .Include(c => c.OrderElements)
               .SingleOrDefaultAsync();
        }
    }
}
