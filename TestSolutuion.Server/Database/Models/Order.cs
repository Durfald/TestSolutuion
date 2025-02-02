using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestSolutuion.Server.Database.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Order
    {
        [Column("ID"), Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ИД заказчика.
        /// </summary>
        [Description("ИД заказчика. Не пустое")]
        [Column("CUSTOMER_ID")]
        [Required]
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

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

        [NotMapped]
        public double Price { get; set; }

        [NotMapped]
        public double PriceWithDiscount { get; set; }

        public Customer Customer { get; set; } = new();

        public ICollection<OrderElement> OrderElements { get; set; } = new List<OrderElement>();

        public const string StatusNew = "Новый";
        public const string StatusInProgress = "Выполняется";
        public const string StatusCompleted = "Выполнен";
    }
}
