
using TestSolutuion.Server.Database.Repository.RCustomer;
using TestSolutuion.Server.Database.Repository.ROrder;
using TestSolutuion.Server.Database.Repository.ROrderElement;
using TestSolutuion.Server.Database.Repository.RProduct;
using TestSolutuion.Server.Database.Repository.RAppUser;

namespace TestSolutuion.Server.Database.Repository.UnitOfWork
{
    public interface IRepositoryUnitOfWork
    {
        public ICustomerRepository Customer { get; }
        public IOrderRepository Order { get; }
        public IOrderElementRepository OrderElement { get; }
        public IProductRepository Product { get; }
        public IAppUserRepository User { get; }
        Task SaveChangesAsync();
    }
}
