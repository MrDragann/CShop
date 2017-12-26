using System.Collections.Generic;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.Web.Models;

namespace CosmeticaShop.Web.Controllers
{
    public class CartController : BaseController
    {
        private readonly IOrderService _orderService = new OrderService();
        private readonly ICartService _cartService = new CartService();
        private readonly IProductService _productService = new ProductService();
        public ActionResult Index()
        {
            var user = new WebUser();
            var model = new CartViewModel
            {
                DiscountProducts = _productService.GetRandomDiscountProducts()
            };
            model.OrderProducts = user.IsAuthorized ? _cartService.GetCart(user.UserId) : _cartService.GetCookieCart();
            return View(model);

        }
        public ActionResult PreparationOrder(List<OrderProductsModel> productsOrder, string couponCode)
        {
            var userId = new WebUser().UserId;
            var response = _orderService.PreparationOrder(userId, productsOrder, couponCode);
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