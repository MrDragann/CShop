using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        #region [ Сервисы ]

        private IOrderService _orderService = new OrderService();

        #endregion

        // GET: Admin/Order
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Список заказов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFilteredOrders(PaginationRequest<BaseFilter> request)
        {
            var model = _orderService.GetFilteredOrders(request);
            return Json(model);
        }

        public ActionResult Details(int orderId)
        {
            var model = _orderService.GetOrderHeaderModel(orderId);
            if (!model.IsSuccess)
                RedirectToAction("Index");
            return View(model.Value);
        }

        [HttpPost]
        public ActionResult ChangeOrderStatus(int orderId, int status)
        {
            var model = _orderService.ChangeOrderStatus(orderId, status);
            return Json(model);
        }
    }
}