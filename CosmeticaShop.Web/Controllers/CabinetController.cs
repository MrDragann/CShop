using System.Web.Mvc;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Models;

namespace CosmeticaShop.Web.Controllers
{
    [AuthorizationAttributePublic]
    public class CabinetController : BaseController
    {
        private readonly IUserService _userService  = new UserService();
        private readonly ISitePageSevice _sitePageSevice = new SitePageSevice();
        private readonly IOrderService _orderService = new OrderService();
        public ActionResult Index()
        {
            var userId = new WebUser().UserId;
            var model = new CabinetViewModel
            {
                User = _userService.GetUser(userId),
                Cities = _sitePageSevice.GetAllCities()
            };       
            return View(model);
        }
     
        /// <summary>
        /// Изменить личные данные
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPersonData(UserDetailModel model)
        {
            var response = _userService.EditPersonData(model);
            return Json(response);
        }

        public ActionResult HistoryOrders()
        {
            var userId = new WebUser().UserId;
            var model = _orderService.GetHistoryOrders(userId);
            return View(model);
        }

        public ActionResult OrderDetail(int id)
        {
            var model = _orderService.GetOrder(id);
            return View(model);
        }

    }
}