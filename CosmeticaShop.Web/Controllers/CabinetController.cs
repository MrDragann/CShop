using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.Web.Infrastructure;
using CosmeticaShop.IServices.Models.User;

namespace CosmeticaShop.Web.Controllers
{
    [AuthorizationAttributePublic]
    public class CabinetController : Controller
    {
  
        public ActionResult Index()
        {
            return View();
        }
     
        /// <summary>
        /// Изменить личные данные
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPersonData(UserDetailModel model)
        {            
            return View();
        }

    }
}