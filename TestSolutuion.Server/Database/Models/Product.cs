using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestSolutuion.Server.Database.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product
    {
        [Column("ID"), Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Код товара, формат «XX-XXXX-YYXX» где Х – число , Y- заглавная буква английского алфавита.
        /// </summary>
        [Description("Код товара, формат «XX-XXXX-YYXX» где Х – число , Y- заглавная буква английского алфавита. Не пустое.")]
        [Column("CODE")]
        [Required]
        [MaxLength(12)]
        [RegularExpression(@"^\d{2}-\d{4}-[A-Z]{2}\d{2}$", ErrorMessage = "Формат кода товара должен быть «XX-XXXX-YYXX», где X – цифры, а Y – заглавные буквы.")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Наименование товара
        /// </summary>
        [Description("Наименование товара")]
        [Column("NAME")]
        public string? Name { get; set; }

        /// <summary>
        /// Цена за единицу
        /// </summary>
        [Description("Цена за единицу")]
        [Column("PRICE")]
        public double Price { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        [Description("Категория товара.")]
        [Column("CATEGORY")]
        [MaxLength(30)]
        public string? Category { get; set; }

        [JsonIgnore]
        public ICollection<OrderElement>? OrderElements { get; set; }
    }
}
