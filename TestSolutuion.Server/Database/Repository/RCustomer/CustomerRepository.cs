using Microsoft.EntityFrameworkCore;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.RCustomer
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SQLiteDataBaseContext context) : base(context) { }

        public async Task<Customer?> GetCustomerWithOrdersAsync(Guid customerId)
        {
            return await _context.Customers
                .Include(c => c.Orders) // Включаем связанные заказы
                .SingleOrDefaultAsync(c => c.Id == customerId);
        }

        public Task<IEnumerable<Customer>> GetCustomersByCodeAsync(string code)
        {
            return FindAsync(x=>x.Code==code);
        }
    }
}
