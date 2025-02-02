using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Domain.CustomerService
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task DeleteCustomerAsync(Guid id);
        Task<Customer> UpdateCustomerAsync(Guid id, Customer customer);
    }
}
