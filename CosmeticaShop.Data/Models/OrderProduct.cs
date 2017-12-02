using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Таблица товаров заказов
    /// </summary>
    public class OrderProduct
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид заказа
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Скидка на товар
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Сумма для платежа
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Ид продукта
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Признак удаления
        /// </summary>
        public DateTime? IsDelete { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с таблицей товаров
        /// </summary>
        public Product Product { get; set; }
        /// <summary>
        /// Связь с таблицей заказов
        /// </summary>
        public OrderHeader Order { get; set; }

        #endregion
    }
}
