using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.SitePage;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    public class SitePageController : Controller
    {
        #region [ Сервисы ]

        private ISitePageSevice _sitePageSevice = new SitePageSevice();

        #endregion


        public ActionResult Edit(EnumSitePage id)
        {
            var model = _sitePageSevice.GetSitePageModel(id);
            return View(model);
        }

        public ActionResult UpdateSitePage(SitePageModel model)
        {
            var response = _sitePageSevice.UpdateSitePage(model);
            return RedirectToAction("Edit", new {id = model.Id});
        }
    }
}