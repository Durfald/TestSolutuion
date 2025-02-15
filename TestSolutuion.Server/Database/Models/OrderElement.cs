using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestSolutuion.Server.Database.Models
{
    /// <summary>
    /// Элемент заказа
    /// </summary>
    public class OrderElement
    {
        [Column("ID"), Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// ИД заказа.
        /// </summary>
        [Description("ИД заказа. Не пустое")]
        [Column("ORDER_ID")]
        [Required]
        [ForeignKey("Order")]
        public string OrderId { get; set; } = string.Empty;

        /// <summary>
        /// ИД товара.
        /// </summary>
        [Description("ИД товара. Не пустое")]
        [Column("ITEM_ID")]
        [Required]
        [ForeignKey("Product")]
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// Количество заказанного товара.
        /// </summary>
        [Description("Количество заказанного товара. Не пустое.")]
        [Column("ITEMS_COUNT")]
        [Required]
        public uint ItemsCount { get; set; }

        /// <summary>
        /// Цена  за единицу.
        /// </summary>
        [Description("Цена  за единицу. Не пустое.")]
        [Column("ITEM_PRICE")]
        [Required]
        public double ItemPrice { get; set; }

        public Order? Order { get; set; }

        public Product? Product { get; set; }
    }
}
