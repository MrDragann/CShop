using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Wish;

namespace CosmeticaShop.Web.Models
{
    public class WishViewModel
    {
        public List<WishModel> Wishes { get; set; }
        public List<ProductBaseModel> Recommends { get; set; }
    }
}