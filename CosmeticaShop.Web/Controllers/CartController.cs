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
    public class CartController : Controller
    {
        private readonly ICartService _cartService = new CartService();
        public ActionResult Index()
        {
            var userId = new WebUser().UserId;
            var model = _cartService.GetCart(userId);
            return View(model);
        }
    }
}