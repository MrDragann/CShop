using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Category;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService = new CategoryService();

        public ActionResult Index()
        {
            var model = _categoryService.GetAllCategories();
            return View(model);
        }

        public ActionResult AddCategory()
        {
            var model = new CategoryEditModel
            {
                Category = new CategoryModel(),
                Categories = _categoryService.GetBaseProductCategories()
            };
            return View("~/Areas/Admin/Views/Category/AddCategory.cshtml", model);
        }

        public ActionResult EditCategory(int id)
        {
            var model = new CategoryEditModel
            {
                Category = _categoryService.GetCategoryModel(id),
                Categories = _categoryService.GetBaseProductCategories()
            };
            return View("~/Areas/Admin/Views/Category/AddCategory.cshtml", model);
        }

        [HttpPost]
        public ActionResult UpdateCategory(CategoryModel model)
        {
            var response = model.Id == 0
                ? _categoryService.AddCategory(model)
                : _categoryService.EditCategory(model);

            return Json(response);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            var response = _categoryService.DeleteCategory(id);
            return Json(response);
        }

        [HttpPost]
        public ActionResult UpCategoryPriority(int? parentId, int categoryId)
        {
            var response = _categoryService.UpCategoryPriority(parentId, categoryId);
            return Json(response);
        }
        [HttpPost]
        public ActionResult DownCategoryPriority(int? parentId, int categoryId)
        {
            var response = _categoryService.DownCategoryPriority(parentId, categoryId);
            return Json(response);
        }
        [HttpPost]
        public ActionResult GetAllCategories()
        {
            var response = _categoryService.GetAllCategories();
            return Json(response);
        }
    }
}