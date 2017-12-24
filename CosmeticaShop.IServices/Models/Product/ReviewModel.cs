using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.IServices.Models.Product
{
    /// <summary>
    /// Модель отзыва
    /// </summary>
    public class ReviewModel

    {   /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public UserDetailModel User { get; set; }

        /// <summary>
        /// Ид товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime DateCreate { get; set; }
        /// <summary>
        /// Дата создание для JS
        /// </summary>
        public long DateCreateJs { get; set; }
        /// <summary>
        /// Содержание
        /// </summary>
        public string Content { get; set; }
     
    }
}
