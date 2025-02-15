using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Database.Repository.UnitOfWork;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.OrderManager
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryUnitOfWork _repositoryUnitOfWork;

        public OrderService(IRepositoryUnitOfWork repositoryUnitOfWork)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
        }

        public async Task ConfirmOrderAsync(string id, DateTime shipmentDate)
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

        public async Task CloseOrderAsync(string id)
        {
            var order = await _repositoryUnitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException("Order not found");
            if(order.Status != Order.StatusInProgress)
                throw new ArgumentException("Order can't be closed");

            order.Status = Order.StatusCompleted;
            await _repositoryUnitOfWork.Order.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(string id)
        {
            var order = await _repositoryUnitOfWork.Order.GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException("Order not found");
            if(order.Status != Order.StatusNew)
                throw new ArgumentException("Order can't be deleted");

            var orderelements = await _repositoryUnitOfWork.OrderElement.GetOrderElementsByOrderIdAsync(id);
            foreach (var item in orderelements)
                await _repositoryUnitOfWork.OrderElement.DeleteAsync(item.Id);

            await _repositoryUnitOfWork.Order.DeleteAsync(id);
        }

        public async Task<Order> CreateOrderAsync(OrderModel ordermodel)
        {
            var Order = new Order();
            Order.CustomerId = ordermodel.user_id;
            Order.OrderNumber = new Random().Next();

            var customer = await _repositoryUnitOfWork.Customer.GetByIdAsync(ordermodel.user_id);
            if(customer == null)
                throw new ArgumentException("Customer not found");
            customer.Address = ordermodel.address;

            await _repositoryUnitOfWork.Customer.UpdateAsync(customer);
            await _repositoryUnitOfWork.Order.AddAsync(Order);

            foreach (var item in ordermodel.elements)
            {
                var orderelement = new OrderElement();
                orderelement.ItemId = item.id;
                orderelement.ItemPrice = item.price;
                orderelement.ItemsCount = item.quantity;
                orderelement.OrderId = Order.Id;

                await _repositoryUnitOfWork.OrderElement.AddAsync(orderelement);
            }
            return Order;
        }

        public async Task<IEnumerable<OrderHistoryModel>> GetHistoryOrderAsync(string customerId)
        {
            List<Order> Orders = new();
            var q = await _repositoryUnitOfWork.Order.GetOrdersByCustomerAsync(customerId);

            foreach (var item in q) {
                var order = await _repositoryUnitOfWork.Order.GetOrderWithOrderElementsAsync(item.Id);
                Orders.Add(order);
            }

            var products = await _repositoryUnitOfWork.Product.GetAllAsync();

            List<OrderHistoryModel> ordersHistory = new();

            foreach(var item in Orders)
            {
                var orderhistory = new OrderHistoryModel();
                orderhistory.id = item.Id;
                orderhistory.OrderNumber = item.OrderNumber;
                orderhistory.OrderDate = item.OrderDate;
                orderhistory.ShipmentDate = item.ShipmentDate;
                orderhistory.Status = item.Status;
                orderhistory.OrderElements = new();
                double price = 0;

                foreach(var element in item.OrderElements)
                {
                    var historyElement = new OrderHistoryElementModel();

                    historyElement.ProductName = products.FirstOrDefault(x=>x.Id == element.ItemId)?.Name?? "Товар не найден";
                    historyElement.Price = element.ItemPrice;
                    historyElement.Quantity = element.ItemsCount;
                    orderhistory.OrderElements.Add(historyElement);
                    price += element.ItemPrice * element.ItemsCount;
                }

                orderhistory.Price = price;

                ordersHistory.Add(orderhistory);
            }
            
            return ordersHistory.OrderByDescending(o => o.OrderDate).ToList();
        }

        public async Task<IEnumerable<OrderHistoryModel>> GetAllOrdersAsync()
        {
            var customers = await _repositoryUnitOfWork.Customer.GetAllAsync();

            List<OrderHistoryModel> ordersHistory = new();

            foreach (var customer in customers)
            {
                var history = await this.GetHistoryOrderAsync(customer.Id);
                ordersHistory.AddRange(history);
            }

            return ordersHistory;
        }
    }

}
