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

        public async Task<Customer> GetCustomerByIdAsync(string id)
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

        public async Task DeleteCustomerAsync(string id)
        {
            var result = await _repositoryUnitOfWork.Customer.DeleteAsync(id);
            if (!result)
                throw new ArgumentException("Customer not found");
        }

        public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
        {
            var dbcustomer = await _repositoryUnitOfWork.Customer.GetByIdAsync(id);
            if (dbcustomer == null)
                throw new ArgumentException("Customer not found");
            dbcustomer.Discount = customer.Discount;
            dbcustomer.Address = customer.Address;
            await _repositoryUnitOfWork.Customer.UpdateAsync(dbcustomer);
            return customer;
        }
    }
}
