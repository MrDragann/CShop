using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models.Product
{
    public class ProductFilterModel
    {
        public int? Page { get; set; }
        /// <summary>
        /// Список брэндов
        /// </summary>
        public List<int> BrandiesId { get; set; }
        /// <summary>
        /// Список категорий
        /// </summary>
        public List<int> CategoriesId { get; set; }
        /// <summary>
        /// Список тэгов
        /// </summary>
        public List<int> TagsId { get; set; }
        /// <summary>
        /// Поиск
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// Скидки
        /// </summary>
        public bool Discount { get; set; }
    }
}
