using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Database.Repository.UnitOfWork;

namespace TestSolutuion.Server.Domain.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepositoryUnitOfWork _repositoryUnitOfWork;

        public CustomerService(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _repositoryUnitOfWork.Customer.GetByIdAsync(id);
            if (customer == null)
                throw new ArgumentException("Customer not found");
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _repositoryUnitOfWork.Customer.GetAllAsync();
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var result = await _repositoryUnitOfWork.Customer.DeleteAsync(id);
            if (!result)
                throw new ArgumentException("Customer not found");
        }

        public async Task<Customer> UpdateCustomerAsync(Guid id, Customer customer)
        {
            var dbcustomer = _repositoryUnitOfWork.Customer.GetByIdAsync(id);
            if (dbcustomer == null)
                throw new ArgumentException("Customer not found");
            customer.Id = id;
            await _repositoryUnitOfWork.Customer.UpdateAsync(customer);
            return customer;
        }
    }
}
