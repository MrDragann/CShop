using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    public class Product
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид бренда
        /// </summary>
        public int? BrandId { get; set; }

        /// <summary>
        /// Ид категории
        /// </summary>
        //todo:не уверен на счет множества категорий у товара
        public int? CategoryId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url товара
        /// </summary>
        public string KeyUrl { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Скидка на товар
        /// </summary>
        public decimal Discount { get; set; }

        #endregion

        #region [ Связанные объекты]

        /// <summary>
        /// Связь с таблицей товаров заказов
        /// </summary>
        public List<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Связь с брендом
        /// </summary>
        public Brand Brand { get; set; }

        /// <summary>
        /// Связь с категорией
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Связь с списком желаемого
        /// </summary>
        public List<WishList> WishLists { get; set; }

        /// <summary>
        /// Связь с списком отзывов
        /// </summary>
        public List<ProductReview> ProductReviews { get; set; }

        #endregion
    }
}
