using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.IServices.Models.Base;

namespace CosmeticaShop.IServices.Models.Product
{
    public class ProductEditViewModel
    {
        /// <summary>
        /// Модель товара
        /// </summary>
        public ProductEditModel Product { get; set; }

        /// <summary>
        /// Список брендов
        /// </summary>
        public List<BaseModel> Brands { get; set; }

        /// <summary>
        /// Список категорий
        /// </summary>
        public List<BaseModel> Categories { get; set; }
    }
}
