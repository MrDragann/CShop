using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Controllers
{
    [AuthorizationAttributePublic]
    public class CabinetController : Controller
    {
        private readonly IUserService _userService  = new UserService();
        public ActionResult Index()
        {
            var userId = new WebUser().UserId;
            var model = _userService.GetUser(userId);
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

    }
}