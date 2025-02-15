using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.ROrderElement
{
    public interface IOrderElementRepository : IRepository<OrderElement>
    {
        /// <summary>
        /// Получить элементы заказа
        /// </summary>
        /// <param name="orderId"> Идентификатор элемента заказа</param>
        /// <returns></returns>
        public Task<IEnumerable<OrderElement>> GetOrderElementsByOrderIdAsync(string orderId);
    }
}
