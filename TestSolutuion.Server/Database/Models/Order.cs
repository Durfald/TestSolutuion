using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestSolutuion.Server.Database.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        [Column("ID"), Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// ИД заказчика.
        /// </summary>
        [Description("ИД заказчика. Не пустое")]
        [Column("CUSTOMER_ID")]
        [Required]
        [ForeignKey("Customer")]
        public string CustomerId { get; set; } = string.Empty;

        /// <summary>
        /// Дата когда сделан заказ.
        /// </summary>
        [Description("Дата когда сделан заказ. Не пустое")]
        [Column("ORDER_DATE")]
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата доставки
        /// </summary>
        [Description("Дата доставки")]
        [Column("SHIPMENT_DATE")]
        public DateTime ShipmentDate { get; set; }

        /// <summary>
        /// Номер заказа
        /// </summary>
        [Description("Номер заказа")]
        [Column("ORDER_NUMBER")]
        public int OrderNumber { get; set; }

        /// <summary>
        /// Состояние заказа
        /// </summary>
        [Description("Состояние")]
        [Column("STATUS")]
        public string? Status { get; set; } = StatusNew;

        public Customer? Customer { get; set; }
        [JsonIgnore]
        public ICollection<OrderElement>? OrderElements { get; set; }

        public const string StatusNew = "Новый";
        public const string StatusInProgress = "Выполняется";
        public const string StatusCompleted = "Выполнен";
    }
}
