using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.ROrder
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// Получить заказы по статусу
        /// </summary>
        /// <param name="status"> Статус заказа</param>
        /// <returns></returns>
        public Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);

        /// <summary>
        /// Получить заказы по клиенту
        /// </summary>
        /// <param name="customerId"> Идентификатор клиента</param>
        /// <returns></returns>
        public Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);

        /// <summary>
        /// Получить заказы в заданном диапазоне
        /// </summary>
        /// <param name="createdBefore"> Начальная дата создания</param>
        /// <param name="createdAfter"> Конечная дата создания</param>
        /// <param name="shippedBefore"> Начальная дата отправки</param>
        /// <param name="shippedAfter"> Конечная дата отправки</param>
        /// <returns></returns>
        public Task<IEnumerable<Order>> GetOrdersWithinDateRangeAsync(
            DateTime? createdBefore = null,
            DateTime? createdAfter = null,
            DateTime? shippedBefore = null,
            DateTime? shippedAfter = null);

        /// <summary>
        /// Получить заказ по номеру
        /// </summary>
        /// <param name="orderNumber"> Номер заказа</param>
        /// <returns></returns>
        public Task<Order?> GetOrderByOrderNumberAsync(int orderNumber);

        /// <summary>
        /// Поучить заказ с элементами заказа
        /// </summary>
        /// <param name="orderId"> Идентификатор заказа</param>
        /// <returns></returns>
        public Task<Order?> GetOrderWithOrderElementsAsync(string orderId);
    }
}
