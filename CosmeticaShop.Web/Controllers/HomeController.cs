﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private IAuthCommonService _authCommonService = new AuthCommonService();

        public ActionResult Index()
        {
            return View();
        }
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
        public ActionResult Login(ModelUserBase model)
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
        public ActionResult Register(ModelUserDetail model)
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
            if (token == Guid.Empty)
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
        public ActionResult ConfrimedRegister(ModelUserDetail model)
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