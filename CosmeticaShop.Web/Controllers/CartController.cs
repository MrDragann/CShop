using System.Collections.Generic;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Controllers
{
    public class CartController : BaseController
    {
        private static readonly IOrderService _orderService = new OrderService();
        private readonly ICartService _cartService = new CartService();
        public ActionResult Index()
        {
            var user = new WebUser();
            if (user.IsAuthorized)
            {
                var model = _cartService.GetCart(user.UserId);
                return View(model);
            }
            var cookieModel = _cartService.GetCookieCart();
            return View(cookieModel);

        }
        public ActionResult PreparationOrder(List<OrderProductsModel> productsOrder)
        {
            var userId = new WebUser().UserId;
            var response = _orderService.PreparationOrder(userId, productsOrder);
            return Json(response);
        }
        public ActionResult DeleteProduct(int productId)
        {
            var userId = new WebUser().UserId;
            var response = _cartService.DeleteProduct(userId, productId);
            return Json(response);
        }
        /// <summary>
        /// Загрузить корзину
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCart()
        {
            var user = new WebUser();
            if (user.IsAuthorized)
            {
                var response = _cartService.GetCart(user.UserId);
                return Json(response);
            }
            var cookieResponse = _cartService.GetCookieCart();
            return Json(cookieResponse);
        }
    }
}