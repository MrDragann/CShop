using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data.Models.Seo;

namespace CosmeticaShop.Data.Models
{
    public class Product : SeoTags
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
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Код товара
        /// </summary>
        public string Code { get; set; }

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
        /// Скидка на товар (в процентах)
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Url изображения
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Рекомендованный
        /// </summary>
        public bool IsRecommended { get; set; }

        /// <summary>
        /// Присутствует на складе
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsActive { get; set; }

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
        /// Связь со списком категорий
        /// </summary>
        public List<Category> Categories { get; set; }

        /// <summary>
        /// Связь с списком желаемого
        /// </summary>
        public List<WishList> WishLists { get; set; }

        /// <summary>
        /// Связь с списком отзывов
        /// </summary>
        public List<ProductReview> ProductReviews { get; set; }

        /// <summary>
        /// Список тегов
        /// </summary>
        public List<ProductTag> ProductTags { get; set; }

        /// <summary>
        /// Похожие товары
        /// </summary>
        public List<Product> SimilarProducts { get; set; }

        /// <summary>
        /// Список товаров
        /// </summary>
        public List<Product> Products { get; set; }

        #endregion
    }
}
