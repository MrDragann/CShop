using System.Collections.Generic;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Order;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Controllers
{
    public class OrderController : BaseController
    {
       private static readonly IOrderService _orderService = new OrderService();
        public ActionResult Index(int orderId)
        {
            var model = _orderService.GetOrder(orderId);
            if (model.Status != (int) EnumStatusOrder.Cart)
                return RedirectToAction("Index", "Home");
            return View(model);
        }
        public ActionResult AddOrder(int orderId,AddressModel address)
        {
            var res = _orderService.AddOrder(orderId,address);
            return Json(res);
        }

    }
}