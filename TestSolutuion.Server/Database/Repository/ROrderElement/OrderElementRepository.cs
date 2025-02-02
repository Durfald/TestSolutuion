using Microsoft.EntityFrameworkCore;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.ROrderElement
{
    public class OrderElementRepository : Repository<OrderElement>, IOrderElementRepository
    {
        public OrderElementRepository(SQLiteDataBaseContext context) : base(context) { }

        public async Task<IEnumerable<OrderElement>> GetOrderElementsByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderElements
                  .Where(oe => oe.OrderId == orderId)  
                  .ToListAsync();
        }
    }
}
