using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Brand;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services;
using CosmeticaShop.Services.Static;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    //[Authorization]
    public class ProductController : Controller
    {
        #region [ Сервисы ]

        private IProductService _productService = new ProductService();
        private ICategoryService _categoryService = new CategoryService();

        #endregion

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddProduct()
        {
            var model = new ProductEditViewModel
            {
                Product = new ProductEditModel(),
                Brands = _productService.GetAllBrandsBase(),
                Categories = _categoryService.GetBaseProductCategories()
            };
            return View(model);
        }

        public ActionResult EditProduct(int id)
        {
            var model = new ProductEditViewModel
            {
                Product = _productService.GetProductModel(id).Value,
                Brands = _productService.GetAllBrandsBase(),
                Categories = _categoryService.GetBaseProductCategories()
            };
            return View("~/Areas/Admin/Views/Product/AddProduct.cshtml",model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UpdateProduct(ProductEditModel model)
        {
            var response = model.Id == 0 
                ? _productService.AddProduct(model)
                : _productService.EditProduct(model);
            return Json(response);
        }

        [HttpPost]
        public ActionResult UploadProductPhotos(HttpPostedFileBase photoFile, int productId)
        {
            FileManager.SaveImage(photoFile, EnumDirectoryType.Product, FileManager.PreviewName, 
                productId.ToString());
            //foreach (string file in photoFiles)
            //{
            //    var upload = photoFiles[file];
            //    var newFileName = Guid.NewGuid() + Path.GetFileNameWithoutExtension(upload.FileName);
            //    FileManager.SaveImage(upload, EnumDirectoryType.Product, newFileName, productId.ToString());
            //}
            return Json("Загрузка завершена");
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