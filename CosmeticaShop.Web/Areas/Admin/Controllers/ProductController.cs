using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
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

        #endregion
    }
}