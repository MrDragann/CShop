using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.User;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IAuthCommonService _authCommonService = new AuthCommonService();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var model = new ModelUserDetail
            {
                Email = "testshop2018@gmail.com",
                FirstName = "Максим",
                LastName = "Звинаревский",
                Country = "ПМР",
                City = "Тирасполь",
                Address = "Краснодонская 82 кв.88",
                Phone = "Galaxy S3",
                Password = "maniac",
                DateBirth = DateTime.Now.AddDays(-10).AddYears(-20),
                RegistrationDate = DateTime.Now
            };
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
            return View();
        }
    }
}