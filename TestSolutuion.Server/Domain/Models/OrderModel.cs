namespace TestSolutuion.Server.Domain.Models
{
    public class OrderModel
    {
        public string user_id { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public List<OrderElementModel>? elements { get; set; }
    }

    public class OrderElementModel
    {
        public string id {  get; set; } = string.Empty;
        public uint quantity { get; set; }
        public double price { get; set; }
    }
}
