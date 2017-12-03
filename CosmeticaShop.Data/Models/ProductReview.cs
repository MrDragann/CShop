using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    public class ProductReview
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Ид товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Содержание
        /// </summary>
        public string Content { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Связь с товаром
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Связь с пользователем
        /// </summary>
        public User User { get; set; }

        #endregion
    }
}
