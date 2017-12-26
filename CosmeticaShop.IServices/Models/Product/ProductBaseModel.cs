using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticaShop.IServices.Models.Product
{
    public class ProductBaseModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Скидка на товар в %
        /// </summary>
        public decimal DiscountPercent { get; set; }
        /// <summary>
        /// Цена со скидкой
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// Url изображения
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Наименование бренда
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// Ид тэга
        /// </summary>
        public List<int> TagsId { get; set; }
    }
}
