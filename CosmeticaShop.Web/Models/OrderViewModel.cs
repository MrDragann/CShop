using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CosmeticaShop.IServices.Models;
using CosmeticaShop.IServices.Models.Order;

namespace CosmeticaShop.Web.Models
{
    public class OrderViewModel
    {
        /// <summary>
        /// Заказ
        /// </summary>
        public OrderHeaderModel Order { get; set; }
        /// <summary>
        /// Список городов
        /// </summary>
        public List<DictionaryModel> Cities { get; set; }
    }
}