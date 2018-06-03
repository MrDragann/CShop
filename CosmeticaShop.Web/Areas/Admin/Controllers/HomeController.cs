using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
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
        public ActionResult Index(string returnUrl)
        {
            var user = new WebUser();
            if (user.IsAdmin)
                return RedirectToAction("Index", "Dashboard");
            if (!(Session["UserSession"] is LoginModel))
            {
                var data = Request.Cookies["UserData"];
                if (data != null)
                {
                    var model = AuthCommonService.Decrypt(data.Value);
                    if (model != null)
                    {
                        var loginStatus = _authCommonService.Login(model.Email, model.Password);
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
                            //var sreturnUrl = HttpContext.Request.UrlReferrer?.AbsoluteUri;
                            if (!string.IsNullOrEmpty(returnUrl))
                                return Redirect(returnUrl);
                            return RedirectToAction("Index");
                        }
                        LogOut();
                    }
                }
            }
            return View(new LoginModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(LoginModel model)
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
                if (model.IsRemember)
                    Response.Cookies.Add(new HttpCookie("UserData")
                    {
                        Value = AuthCommonService.Encrypt(new LoginModel
                        {
                            Email = model.Email,
                            Password = passwordHash
                        }),
                        Expires = DateTime.Now.AddDays(7)
                    });

                var returnUrl = HttpContext.Request.UrlReferrer?.AbsoluteUri.Replace("/Admin/Home/Index?returnUrl=", "");
                return Redirect(returnUrl);
            }
            return View(new LoginModel());
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("UserSession");
            Response.Cookies.Add(new HttpCookie("UserData") { Expires = DateTime.Now.AddDays(-1) });
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создает перемнную в которой находится путь для сохранения файла и далее выполняет метод сохраняющий изображения 
        /// </summary>
        /// <param name="upload">Файл</param>
        /// <param name="ckEditorFuncNum">Идентификационный номер анонимной функции обратного вызова после загрузки</param>
        /// <returns></returns>
        public ActionResult UploadImage(HttpPostedFileBase upload, string ckEditorFuncNum)
        {
            var host = HttpContext.Request.Url?.Authority;
            var fileName = Guid.NewGuid() + System.IO.Path.GetFileNameWithoutExtension(upload.FileName);
            var filePath = FileManager.SaveImage(upload, EnumDirectoryType.Upload, fileName);

            var url = $"http://{host}/{ filePath}";
            var message = "Ваше изображение успешно загружено";
            // Ajax запрос для передачи изображения
            var output = $"<html><body><script>window.parent.CKEDITOR.tools.callFunction({ckEditorFuncNum}, \"{url}\", \"{message}\");</script></body></html>";
            return Content(output);
        }
    }
}