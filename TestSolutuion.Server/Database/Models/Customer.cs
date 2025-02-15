using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestSolutuion.Server.Database.Models
{
    /// <summary>
    /// Заказчик
    /// </summary>
    public class Customer
    {
        [Column("ID"), Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Наименование заказчика.
        /// </summary>
        [Description("Наименование заказчика. Не пустое")]
        [Column("NAME")]
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// <para>Код заказчика. Содержит данные формата «ХХХХ-ГГГГ»</para>
        /// <para>где Х – число, ГГГГ – год в котором зарегистрирован заказчик.</para>
        /// </summary>
        [Description("Код заказчика. Содержит данные формата «ХХХХ-ГГГГ» где Х – число, ГГГГ – год в котором зарегистрирован заказчик. Не пустое.")]
        [Column("CODE")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Формат должен быть 'ХХХХ-ГГГГ'")]
        [Length(9, 9, ErrorMessage = "Код должен иметь длину 9 символов")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Адрес заказчика
        /// </summary>
        [Description("Адрес заказчика")]
        [Column("ADDRESS")]
        public string? Address { get; set; }


        /// <summary>
        /// % скидки для заказчика.
        /// <para>0 или null – означает что скидка не распространяется.</para>
        /// </summary>
        [Description("% скидки для заказчика. 0 или null – означает что скидка не распространяется.")]
        [Column("DISCOUNT")]
        public float? Discount { get; set; }
        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }
    }
}