using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    [Authorization]
    public class ProductController : Controller
    {
        #region [ Сервисы ]

        private IProductService _productService = new ProductService();

        #endregion

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        #region [ Бренды ]

        public ActionResult Brands()
        {
            return View("~/Areas/Admin/Views/Brand/Brands.cshtml");
        }

        [HttpPost]
        public ActionResult GetFilteredBrands(PaginationRequest<BaseFilter> request)
        {
            var response = _productService.GetFilteredBrands(request);
            return Json(response);
        }

        public ActionResult AddBrand()
        {
            var model = new BaseResponse<BrandModel>(new BrandModel());
            return View("~/Areas/Admin/Views/Brand/AddBrand.cshtml",model);
        }

        [HttpPost]
        public ActionResult AddBrand(BrandModel model)
        {
            var response = _productService.AddBrand(model);
            if (response.IsSuccess)
                return RedirectToAction("Brands");
            return View("~/Areas/Admin/Views/Brand/AddBrand.cshtml",(BaseResponse<BrandModel>) response);
        }

        public ActionResult EditBrand(int id)
        {
            var model = _productService.GetBrandModel(id);
            if(model.IsSuccess)
                return View("~/Areas/Admin/Views/Brand/AddBrand.cshtml", model);
            return RedirectToAction("Brands");
        }

        [HttpPost]
        public ActionResult EditBrand(BrandModel model)
        {
            var response = _productService.AddBrand(model);
            if (response.IsSuccess)
                return RedirectToAction("Brands");
            return View("~/Areas/Admin/Views/Brand/AddBrand.cshtml", (BaseResponse<BrandModel>)response);
        }

        [HttpPost]
        public ActionResult DeleteBrand(int brandId)
        {
            var response = _productService.DeleteBrand(brandId);
            return Json(response);
        }

        #endregion
    }
}