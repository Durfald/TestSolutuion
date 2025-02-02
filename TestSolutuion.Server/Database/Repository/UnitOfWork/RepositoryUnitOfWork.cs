using TestSolutuion.Server.Database.Repository.RCustomer;
using TestSolutuion.Server.Database.Repository.ROrder;
using TestSolutuion.Server.Database.Repository.ROrderElement;
using TestSolutuion.Server.Database.Repository.RProduct;
using TestSolutuion.Server.Database.Repository.RAppUser;

namespace TestSolutuion.Server.Database.Repository.UnitOfWork
{
    public class RepositoryUnitOfWork : IRepositoryUnitOfWork
    {
        public ICustomerRepository Customer { get; }

        public IOrderRepository Order { get; }

        public IOrderElementRepository OrderElement { get; }

        public IProductRepository Product { get; }

        public IAppUserRepository User { get; }

        private readonly SQLiteDataBaseContext _context;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public RepositoryUnitOfWork(SQLiteDataBaseContext context)
        {
            this._context = context;
            Customer = new CustomerRepository(_context);
            Order = new OrderRepository(_context);
            OrderElement = new OrderElementRepository(_context);
            Product = new ProductRepository(_context);
            User = new AppUserRepository(_context);
        }
    }
}
