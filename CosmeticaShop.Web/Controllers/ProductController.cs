using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosmeticaShop.Web.Controllers
{
    public class ProductController : Controller
    {
        /// <summary>
        /// Страница с списоком товаров
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Детальная страница товара
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            return View();
        }
    }
}