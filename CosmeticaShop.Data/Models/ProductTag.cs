using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    /// <summary>
    /// Таблица тегов товаров
    /// </summary>
    public class ProductTag
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

        #endregion

        #region [ Связанные объекты ]

        /// <summary>
        /// Список товаров
        /// </summary>
        public List<Product> Products { get; set; }

        #endregion
    }
}
