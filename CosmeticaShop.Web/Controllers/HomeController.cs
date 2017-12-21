using System;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services;
using CosmeticaShop.Services.Static;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region [ Сервисы ]

        private IAuthCommonService _authCommonService = new AuthCommonService();
        private ISitePageSevice _sitePageSevice = new SitePageSevice();

        #endregion

        public ActionResult Index()
        {
            SetSitePageSettings(EnumSitePage.Home);
            return View();
        }

        #region [ Шаблоны ]

        public ActionResult NavigationPartial()
        {
            var model = _sitePageSevice.GetSiteNavigation();
            return PartialView("~/Views/Shared/Partial/Navigation.cshtml", model);
        }

        /// <summary>
        /// Получить слайдер
        /// </summary>
        /// <returns></returns>
        public ActionResult SliderPartial()
        {
            var model = _sitePageSevice.GetSlides();
            return PartialView("~/Views/Shared/Partial/SitePage/Slider.cshtml", model);
        }

        public ActionResult SitePageSettings(EnumSitePage page)
        {
            var model = _sitePageSevice.GetSitePageModel(page);
            return PartialView("~/Views/Shared/Partial/SitePage/SitePageSettings.cshtml", model);
        }

        #endregion

        public ActionResult Contacts()
        {
            return View();
        }
        #region Авторизация
        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="model">Модель пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(UserBaseModel model)
        {
            var passwordHash = model.Password.GetHashString();

            var loginStatus = _authCommonService.Login(model.Email, passwordHash);
            //todo:получать returnUrl параметром
            var returnUrl = HttpContext.Request.UrlReferrer.AbsoluteUri.Replace("/Admin/Home/Login?returnUrl=", "");

            if (loginStatus.IsSuccess)
            {
                var webUser = new WebUser()
                {
                    Email = loginStatus.Value.Email,
                    IsAuthorized = true,
                    UserId = loginStatus.Value.Id,
                    FirstName = loginStatus.Value.FirstName,
                    LastName = loginStatus.Value.LastName
                };
                System.Web.HttpContext.Current.Session["UserSession"] = webUser;
                return Json(new BaseResponse<string>(0, "Успешно", returnUrl));
            }
            return Json(loginStatus);
        }

        /// <summary>
        /// Post запрос для регистрации
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(UserDetailModel model)
        {

            model.Password = model.Password.GetHashString();
            var token = Guid.NewGuid();
            var registeredStatus = _authCommonService.Register(model, token);
            if (registeredStatus.IsSuccess)
            {

                var mailResponse = _authCommonService.SendMail("Подтвердите регистрацию", model.Email, $@"Для завершения регистрации перейдите по 
                 <a href='{Url.Action("Confrimed", "Home", new { token = token, email = model.Email }, Request.Url.Scheme)}'
                 title='Подтвердить регистрацию'>ссылке</a>");
                if (mailResponse.IsSuccess)
                {
                    return Json(mailResponse);
                }
            }
            return Json(registeredStatus);
        }
        /// <summary>
        /// Подтверждение аккаунта
        /// </summary>
        /// <param name="token">токен</param>
        /// <param name="email">Почта пользователя</param>
        public ActionResult Confrimed(Guid token, string email)
        {
            if (token != Guid.Empty)
            {
                var response = _authCommonService.ConfrimUser(token, email);
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", "Home", new { userId = response.Value });
                }
            }
            return View("Error");
        }
        /// <summary>
        /// Подтверждение регистрации аккаунта
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public ActionResult ConfrimedRegister(UserDetailModel model)
        {
            var response = _authCommonService.ConfrimRegisterUser(model);
            return Json(response);
        }
        /// <summary>
        /// Выход
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            System.Web.HttpContext.Current.Session.Remove("UserSession");
            return RedirectToAction("Index");
        }
        #endregion
    }
}