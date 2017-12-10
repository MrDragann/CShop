using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService = new ProductService();
        private readonly IOrderService _orderService = new OrderService();
        /// <summary>
        /// Страница с списоком товаров
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <param name="quantity">Количество товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInCart(int productId,int quantity)
        {
            var userId = new WebUser().UserId;
            var response = _orderService.AddProductInCart(productId, userId, quantity);
            return Json(response);
        }
        /// <summary>
        /// Добавить товар в желаемое
        /// </summary>
        /// <param name="productId">Ид товара</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInWish(int productId)
        {
            var userId = new WebUser().UserId;
            var response = _productService.AddProductInWish(productId, userId);
            return Json(response);
        }
        /// <summary>
        /// Детальная страница товара
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            return View();
        }
    }
}