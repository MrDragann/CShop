using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CosmeticaShop.IServices.Models.Product
{
    public class ProductEditModel
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
        /// Название брэнда
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url товара
        /// </summary>
        public string KeyUrl { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

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

        /// <summary>
        /// Url изображения
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Присутствует на складе
        /// </summary>
        public bool IsInStock { get; set; }

        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Список Ид категорий
        /// </summary>
        public List<int> CategoriesId { get; set; }

        /// <summary>
        /// Список Ид тегов
        /// </summary>
        public List<int> TagsId { get; set; }

        public HttpPostedFileBase PhotoFile { get; set; }

        public HttpPostedFileBase PhotoName { get; set; }

        #endregion
    }
}
