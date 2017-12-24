using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Models
{
    public class HomeViewModel
    {
        /// <summary>
        /// 4 случайных брэнда
        /// </summary>
        public  List<BrandModel> Brands { get; set; }
        public List<ProductBaseModel> BestSellers { get; set; }
    }
}