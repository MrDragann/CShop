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
        private readonly IOrderService _orderService = new OrderService();
        private readonly IAuthCommonService _authCommonService = new AuthCommonService();
        private readonly IUserService _userService = new UserService();
        public ActionResult Index(int orderId)
        {
            var user = new WebUser();
            if (!user.IsAuthorized)
            {
                var userCookie = _userService.GetUserCookie();
                if (userCookie.IsSuccess)
                {
                    var webUser = new WebUser()
                    {
                        Email = userCookie.Value.Email,
                        IsAuthorized = false,
                        UserId = userCookie.Value.Id,
                        FirstName = userCookie.Value.FirstName,
                        LastName = userCookie.Value.LastName,
                        IsUnAuthorizedUser = true
                    };
                    System.Web.HttpContext.Current.Session["UserSession"] = webUser;
                }
            }
            var model = _orderService.GetOrder(orderId);
            if (model.Status != (int)EnumStatusOrder.Cart)
                return RedirectToAction("Index", "Home");
            return View(model);
        }
        public ActionResult AddOrder(int orderId, AddressModel address, string email)
        {
            var user = new WebUser();
            if (user.IsAuthorized || user.IsUnAuthorizedUser)
            {
                var res = _orderService.AddOrder(orderId, address, email, user.UserId);
                return Json(res);
            }
            else
            {
                var res = _orderService.AddOrder(orderId, address, email, null);
                if (res.IsSuccess && res.Value.IsNewUser)
                {
                    var mailResponse = _authCommonService.SendMail("Подтвердите регистрацию", email, $@"Ваш пароль от интрнет магазина:'{res.Value.Password}'.Для завершения регистрации перейдите по 
                 <a href='{Url.Action("Confrimed", "Home", new { token = res.Value.Token, email = email }, Request.Url.Scheme)}'
                 title='Подтвердить регистрацию'>ссылке</a>");
                    return Json(mailResponse);
                }
                return Json(res);
            }
        }

    }
}