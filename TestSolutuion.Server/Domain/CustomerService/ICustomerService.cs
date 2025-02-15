using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Domain.CustomerService
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByIdAsync(string id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task DeleteCustomerAsync(string id);
        Task<Customer> UpdateCustomerAsync(string id, Customer customer);
    }
}
