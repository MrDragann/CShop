using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services;
using CosmeticaShop.Services.Static;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IAuthCommonService _authCommonService = new AuthCommonService();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var user = new WebUser();
            if (user.IsAdmin)
                return RedirectToAction("Index", "Dashboard");

            return View(new UserLoginModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(UserLoginModel model)
        {
            var passwordHash = model.Password.GetHashString();
            var loginStatus = _authCommonService.Login(model.Email, passwordHash);
            if (loginStatus.IsSuccess)
            {
                var webUser = new WebUser
                {
                    UserId = loginStatus.Value.Id,
                    Email = loginStatus.Value.Email,
                    Roles = loginStatus.Value.Roles,
                    IsAuthorized = true
                };
                HttpContext.Session["UserSession"] = webUser;
                var returnUrl = HttpContext.Request.UrlReferrer.AbsoluteUri.Replace("/Admin/Home/Index?returnUrl=", "");
                return Redirect(returnUrl);
            }
            return View(new UserLoginModel());
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("Index");
        }
    }
}