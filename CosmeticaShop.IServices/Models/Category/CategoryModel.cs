using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace CosmeticaShop.IServices.Models.Category
{
    public class CategoryModel
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
        /// Видимость
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Родительская категория
        /// </summary>
        [JsonIgnore]
        public CategoryModel Parent { get; set; }
        /// <summary>
        /// Список дочерних категорий
        /// </summary>
        public List<CategoryModel> ChildCategories { get; set; }

        #endregion
    }
}
