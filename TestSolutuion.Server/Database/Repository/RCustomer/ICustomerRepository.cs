using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.RCustomer
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Получить заказчика с заказами
        /// </summary>
        /// <param name="customerId"> Идентификатор заказчика</param>
        /// <returns></returns>
        Task<Customer?> GetCustomerWithOrdersAsync(string customerId);

        /// <summary>
        /// Получить заказчиков по коду
        /// </summary>
        /// <param name="code"> Код</param>
        /// <returns></returns>
        Task<IEnumerable<Customer>> GetCustomersByCodeAsync(string code);
    }
}
