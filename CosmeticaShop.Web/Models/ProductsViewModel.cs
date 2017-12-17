using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Category;
using CosmeticaShop.IServices.Models.Pagination;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsViewModel
    {
        public List<ProductBaseModel> Products { get; set; }    
        public PaginationModel Pagination { get; set; }
        /// <summary>
        /// Cписок всех брэндов
        /// </summary>
        public  List<BaseModel> Brands { get; set; }
        /// <summary>
        /// Cписок всех категорий
        /// </summary>
        public List<CategoryModel> Categories { get; set; }
        /// <summary>
        /// Фильтрация
        /// </summary>
        public  ProductFilterModel Filter { get; set; }
    }
}