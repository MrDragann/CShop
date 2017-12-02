using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.Data.Models
{
    public class Category
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
