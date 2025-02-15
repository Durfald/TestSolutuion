using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.OrderManager
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderHistoryModel>> GetAllOrdersAsync();
        Task ConfirmOrderAsync(string id, DateTime shipmentDate);
        Task CloseOrderAsync(string id);
        Task DeleteOrderAsync(string id);
        Task<Order> CreateOrderAsync(OrderModel order);

        Task<IEnumerable<OrderHistoryModel>> GetHistoryOrderAsync(string customerId);
    }
}
