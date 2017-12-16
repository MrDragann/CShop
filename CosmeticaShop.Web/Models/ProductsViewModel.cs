using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductsViewModel
    {
        public List<ProductBaseModel> Products { get; set; }      
    }
}