using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Database.Repository.UnitOfWork;

namespace TestSolutuion.Server.Domain.OrderManager
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryUnitOfWork _repositoryUnitOfWork;

        public OrderService(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public async Task ConfirmOrderAsync(Guid id, DateTime shipmentDate)
        {
            var order = await _repositoryUnitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException("Order not found");

            if(order.Status != Order.StatusNew)
                throw new ArgumentException("Order can't be confirmed");

            order.ShipmentDate = shipmentDate;
            order.Status = Order.StatusInProgress;

            await _repositoryUnitOfWork.Order.UpdateAsync(order);
        }

        public async Task CloseOrderAsync(Guid id)
        {
            var order = await _repositoryUnitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException("Order not found");
            if(order.Status != Order.StatusInProgress)
                throw new ArgumentException("Order can't be closed");

            order.Status = Order.StatusCompleted;
            await _repositoryUnitOfWork.Order.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _repositoryUnitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException("Order not found");
            if(order.Status != Order.StatusNew)
                throw new ArgumentException("Order can't be deleted");

            await _repositoryUnitOfWork.Order.DeleteAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.Status = Order.StatusNew;
            await _repositoryUnitOfWork.Order.AddAsync(order);

            return order;
        }
    }

}
