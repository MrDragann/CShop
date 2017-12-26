using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.Product;

namespace CosmeticaShop.Web.Models
{
    public class CartViewModel
    {
        public List<OrderProductsModel> OrderProducts { get; set; }
        public List<ProductBaseModel> DiscountProducts { get; set; }
    }
}