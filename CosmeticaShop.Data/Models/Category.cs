using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data.Models.Seo;

namespace CosmeticaShop.Data.Models
{
    public class Category:SeoTags
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ид родителя
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url категории
        /// </summary>
        public string KeyUrl { get; set; }

        /// <summary>
        /// Приоритет
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Url изображения
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Видимость
        /// </summary>
        public bool IsActive { get; set; }

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Родитель
        /// </summary>
        public Category Parent { get; set; }

        /// <summary>
        /// Дочерние категории
        /// </summary>
        public List<Category> ChildCategories { get; set; }

        /// <summary>
        /// Связь с товарами
        /// </summary>
        public List<Product> Products { get; set; }

        #endregion
    }
}
