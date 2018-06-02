using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CosmeticaShop.IServices.Interfaces;
using CosmeticaShop.IServices.Models.Base;
using CosmeticaShop.IServices.Models.Blog;
using CosmeticaShop.IServices.Models.Requests;
using CosmeticaShop.IServices.Models.Responses;
using CosmeticaShop.Services;
using CosmeticaShop.Web.Infrastructure;

namespace CosmeticaShop.Web.Areas.Admin.Controllers
{
    [Authorization(Roles = ConstRoles.Admin)]
    public class BlogController : Controller
    {
        #region [ Сервисы ]

        private IBlogService _blogService = new BlogService();

        #endregion

        #region [ Бренды ]

        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/Blog/Index.cshtml");
        }

        [HttpPost]
        public ActionResult GetFilteredBlogPosts(PaginationRequest<BaseFilter> request)
        {
            var response = _blogService.GetFilteredBlogs(request);
            return Json(response);
        }

        public ActionResult AddBlogPost()
        {
            var model = new BlogModel();
            return View("~/Areas/Admin/Views/Blog/AddBlog.cshtml", model);
        }

        public ActionResult EditBlogPost(int id)
        {
            var model = _blogService.GetBlogPostModel(id);
            if (model.IsSuccess)
                return View("~/Areas/Admin/Views/Blog/AddBlog.cshtml", model.Value);
            return RedirectToAction("Index");
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult BlogPostUpdate(BlogModel model)
        {
            var response = model.Id == 0
                ? _blogService.BlogPostAdd(model)
                : _blogService.BlogPostEdit(model);
            if (response.IsSuccess)
                return RedirectToAction("EditBlogPost",new{id=response.Value});
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteBlog(int blogId)
        {
            var response = _blogService.BlogPostDelete(blogId);
            return Json(response);
        }

        #endregion
    }
}