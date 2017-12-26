using System;
using System.Web.Mvc;
using CosmeticaShop.IServices;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services;
using CosmeticaShop.Services.Static;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.Web.Models;

namespace CosmeticaShop.Web.Controllers
{
    
    public class HomeController : BaseController
    {
        private  readonly IProductService _productService = new ProductService();
        #region [ Сервисы ]

        private readonly IAuthCommonService _authCommonService = new AuthCommonService();
        private readonly ISitePageSevice _sitePageSevice = new SitePageSevice();
        private readonly IWishService _wishService = new WishService();
        private readonly IUserService _userService = new UserService();
        #endregion

        public ActionResult Index(Guid? token, string email)
        {
            var model = new HomeViewModel()
            {
                BestSellers = _productService.GetBestSellers(),
                Brands = _productService.GetBrands(),
                Recommends = _productService.GetRecomendProducts(12)
            };
            SetSitePageSettings(EnumSitePage.Home);
            return View(model);
        }

        [HttpPost]
        public ActionResult GetAllCities()
        {
            var model = _sitePageSevice.GetAllCities();
            return Json(model);
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
                _wishService.ComplementWishs(webUser.UserId);
                _userService.SetUserCookie(webUser.UserId);
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

        /// <summary>
        /// Сбросить пароль
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            var response = _authCommonService.CheckExistEmail(email);
            if (response.IsSuccess)
            {
                var token = _authCommonService.GenerateToken(email);
                if (token.IsSuccess)
                {
                    // Отправка письма с ссылкой на сброс пароля
                    _authCommonService.SendMail("Восстановление пароля", email,
                        $@"Для восстановления  пароля перейдите по 
                 <a href='{
                                Url.Action("Index", "Home", new { token = token.Value.Value, email = email },
                                    Request.Url.Scheme)
                            }'
                 title='Востоновления пароля'>ссылке</a>");
                }
                return Json(new { IsSuccess = true });
            }
            return Json(response);
        }
           /// <summary>
        /// Сменить пароль
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(Guid token, string email, string password)
           {
               password = password.GetHashString();
            var response = _authCommonService.RestorePassword(email, token, password);
            return Json(response);
        }
        #endregion
    }
}