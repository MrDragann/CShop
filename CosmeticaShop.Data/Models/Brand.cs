using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data.Models.Seo;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Таблица брендов
    /// </summary>
    public class Brand : SeoTags
    {
        #region [ Свойства ]

        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url бренда
        /// </summary>
        public string KeyUrl { get; set; }

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
        /// Связь с товарами
        /// </summary>
        public List<Product> Products { get; set; }

        #endregion
    }
}
