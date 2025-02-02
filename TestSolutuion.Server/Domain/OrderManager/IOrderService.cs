using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Domain.OrderManager
{
    public interface IOrderService
    {
        Task ConfirmOrderAsync(Guid id, DateTime shipmentDate);
        Task CloseOrderAsync(Guid id);
        Task DeleteOrderAsync(Guid id);
        Task<Order> CreateOrderAsync(Order order);
    }
}
