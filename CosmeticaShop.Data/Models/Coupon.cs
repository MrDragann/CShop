using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Модель таблицы купонов
    /// </summary>
    public class Coupon
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код купона
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Скидка в %
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Дата создания 
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public DateTime? IsDelete { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с заказами
        /// </summary>
        public List<OrderHeader> OrderHeaders { get; set; }

        #endregion
    }
}
