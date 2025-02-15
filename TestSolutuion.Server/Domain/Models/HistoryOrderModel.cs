namespace TestSolutuion.Server.Domain.Models
{
    public class OrderHistoryModel
    {
        public string id { get; set; } = string.Empty;
        public int OrderNumber { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderHistoryElementModel> OrderElements { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
    }

    public class OrderHistoryElementModel
    {
        public string ProductName { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
    }
}
