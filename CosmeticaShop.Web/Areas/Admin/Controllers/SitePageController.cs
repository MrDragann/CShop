using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.IServices.Models.SitePage;
using CosmeticaShop.IServices.Models.Slider;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    public class SitePageController : Controller
    {
        #region [ Сервисы ]

        private ISitePageSevice _sitePageSevice = new SitePageSevice();

        #endregion

        #region [ Найстройки страниц ]

        public ActionResult Edit(EnumSitePage id)
        {
            var model = _sitePageSevice.GetSitePageModel(id);
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UpdateSitePage(SitePageModel model)
        {
            var response = _sitePageSevice.UpdateSitePage(model);
            return RedirectToAction("Edit", new {id = model.Id});
        }

        #endregion

        #region [ Слайдер ]

        public ActionResult Slider()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetFilteredSlides(PaginationRequest<BaseFilter> request)
        {
            var response = _sitePageSevice.GetFilteredSlides(request);
            return Json(response);
        }

        public ActionResult AddSlide()
        {
            var model = new BaseResponse<SliderEditModel>(new SliderEditModel());
            return View(model);
        }

        public ActionResult EditSlide(int id)
        {
            var model = _sitePageSevice.GetSlideModel(id);
            return View("~/Areas/Admin/Views/SitePage/AddSlide.cshtml", model);
        }
        
        [HttpPost]
        public ActionResult UpdateSlide(SliderEditModel model)
        {
            var response = model.Id == 0
                ? _sitePageSevice.AddSlide(model)
                : _sitePageSevice.EditSlide(model);
            return RedirectToAction("EditSlide",new {id=response.Value});
        }

        [HttpPost]
        public ActionResult DeleteSlide(int id)
        {
            var response = _sitePageSevice.DeleteSlide(id);
            return Json(response);
        }

        #endregion
    }
}